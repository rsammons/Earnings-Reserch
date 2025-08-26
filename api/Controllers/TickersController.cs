using Api.Data;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Services.Clients.Dolthub;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TickersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IDolthubClient _dolthub;

    public TickersController(AppDbContext db, IDolthubClient dolthub) => (_db, _dolthub) = (db, dolthub);

    [HttpPost("from-dolthub")] // POST /api/tickers/from-dolthub?date=YYYY-MM-DD
    public async Task<ActionResult<object>> ImportFromDolthub([FromQuery] DateOnly date, CancellationToken ct)
    {
        if (date == default) return BadRequest("Query parameter 'date' (YYYY-MM-DD) is required.");

        // 1) Fetch from external API
        var source = await _dolthub.GetByDateAsync(date, ct);
        if (!source.Any())
        {
            return Ok(new { added = 0, updated = 0, date, rows = Array.Empty<Ticker>() });
        }

        // Normalize symbols to uppercase (defensive)
        foreach (var s in source)
        {
            s.Symbol = (s.Symbol ?? string.Empty).ToUpperInvariant();
            s.ReleaseDate = date; // enforce the date we asked for
        }

        // 2) Load existing rows for that date *and* those symbols (one round-trip)
        var symbols = source.Select(s => s.Symbol).Distinct().ToArray();
        var existing = await _db.Tickers
            .Where(t => t.ReleaseDate == date && symbols.Contains(t.Symbol))
            .ToListAsync(ct);

        var map = existing.ToDictionary(t => t.Symbol, StringComparer.OrdinalIgnoreCase);

        // 3) Upsert
        int added = 0, updated = 0;
        foreach (var s in source)
        {
            if (map.TryGetValue(s.Symbol, out var row))
            {
                // UPDATE existing
                row.ReleaseSession = s.ReleaseSession;       // overwrite with latest
                                                             // keep row.Tradable as-is (user choice); change if you want: row.Tradable = s.Tradable;
                updated++;
            }
            else
            {
                // INSERT new
                _db.Tickers.Add(new Ticker
                {
                    Symbol = s.Symbol,
                    ReleaseDate = date,
                    ReleaseSession = s.ReleaseSession,
                    Tradable = true, // default; adjust if you want a rule here
                });
                added++;
            }
        }

        await _db.SaveChangesAsync(ct);

        // 4) Return what’s now in DB for that date
        var rows = await _db.Tickers
            .Where(t => t.ReleaseDate == date)
            .OrderBy(t => t.Symbol)
            .ToListAsync(ct);

        return Ok(new { added, updated, date, rows });
    }

    [HttpGet("{id:int}/stats")]
    public async Task<ActionResult<IEnumerable<StockStatsSnapshot>>> GetStats(int id, [FromQuery] DateOnly? asOf, CancellationToken ct)
    {
        Console.WriteLine(id);
        var q = _db.StockStatsSnapshots.Where(s => s.TickerId == id);
        if (asOf is not null) q = q.Where(s => s.AsOfDate == asOf);
        var rows = await q.OrderByDescending(s => s.AsOfDate).Take(1).ToListAsync(ct);

        return Ok(rows);
    }

    [HttpGet] // /api/tickers?date=2025-08-26
    public async Task<ActionResult<IEnumerable<Ticker>>> Get([FromQuery] DateOnly? date, CancellationToken ct)
    {
        var targetDate = date ?? GetNextBusinessDay(DateTime.Now);

        IQueryable<Ticker> q = _db.Tickers;
        q = q.Where(t => t.ReleaseDate == targetDate);
        var list = await q.OrderBy(t => t.Symbol).ToListAsync(ct);
        return Ok(list);
    }

    private static DateOnly GetNextBusinessDay(DateTime from)
    {
        var next = from.Date.AddDays(1);

        // roll forward until not Saturday/Sunday
        while (next.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
        {
            next = next.AddDays(1);
        }

        return DateOnly.FromDateTime(next);
    }



    // GET /api/tickers/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Ticker>> GetOne(int id)
    {
        var t = await _db.Tickers.FindAsync(id);
        return t is null ? NotFound() : Ok(t);
    }

    // POST /api/tickers
    [HttpPost]
    public async Task<ActionResult<Ticker>> Create([FromBody] Ticker t)
    {
        if (string.IsNullOrWhiteSpace(t.Symbol))
            return BadRequest("Symbol is required.");

        t.Symbol = t.Symbol.ToUpperInvariant();
        _db.Tickers.Add(t);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = t.Id }, t);
    }

    // PUT /api/tickers/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Ticker updated)
    {
        var existing = await _db.Tickers.FindAsync(id);
        if (existing is null) return NotFound();

        existing.Symbol = (updated.Symbol ?? existing.Symbol).ToUpperInvariant();
        //existing.EarningRelease = updated.EarningRelease;
        existing.Tradable = updated.Tradable;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // PATCH /api/tickers/{id}/toggle-tradable
    [HttpPatch("{id:int}/toggle-tradable")]
    public async Task<IActionResult> ToggleTradable(int id)
    {
        var t = await _db.Tickers.FindAsync(id);
        if (t is null) return NotFound();
        t.Tradable = !t.Tradable;
        await _db.SaveChangesAsync();
        return Ok(new { t.Id, t.Tradable });
    }

    // DELETE /api/tickers/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _db.Tickers.FindAsync(id);
        if (t is null) return NotFound();
        _db.Tickers.Remove(t);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
