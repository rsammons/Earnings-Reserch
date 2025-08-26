using Api.Models;
using Api.Services.Clients.Finnhub;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketDataController : ControllerBase
{
    private readonly IFinnhubClient _finnhub;
    public MarketDataController(IFinnhubClient finnhub) => _finnhub = finnhub;

    // GET /api/marketdata/stats/AAPL
    [HttpGet("stats/{symbol}")]
    public async Task<ActionResult<StockStatsDto>> GetStats(string symbol, CancellationToken ct)
    {
        var stats = await _finnhub.GetStatsAsync(symbol, ct);
        return stats is null ? NotFound() : Ok(stats);
    }
}
