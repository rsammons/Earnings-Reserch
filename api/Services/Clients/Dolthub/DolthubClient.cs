using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Api.Models;

namespace Api.Services.Clients.Dolthub;

public class DolthubClient(HttpClient http) : IDolthubClient
{
    // DTOs that match DoltHub’s JSON shape
    private sealed record DoltResponse([property: JsonPropertyName("rows")] List<Row> Rows);
    private sealed record Row(
        [property: JsonPropertyName("act_symbol")] string ActSymbol,
        [property: JsonPropertyName("date")] string Date,       // "2025-08-26"
        [property: JsonPropertyName("when")] string When        // e.g., "Before Open" / "After Close"
    );

    public async Task<IEnumerable<Ticker>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
    {
        var sql = $"SELECT act_symbol, date, `when` FROM earnings_calendar WHERE date = '{date:yyyy-MM-dd}' ORDER BY act_symbol ASC;";
        var url = $"?q={Uri.EscapeDataString(sql)}";

        var resp = await http.GetFromJsonAsync<DoltResponse>(url, ct) ?? new DoltResponse([]);

        return resp.Rows.Select(r => new Ticker
        {
            Symbol = r.ActSymbol?.ToUpperInvariant() ?? "",
            ReleaseDate = DateOnly.TryParse(r.Date, out var d) ? d : date,
            ReleaseSession = r.When,
            Tradable = false
        });
    }
}
