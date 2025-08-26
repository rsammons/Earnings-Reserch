using System.Net.Http.Json;
using Api.Models;

namespace Api.Services.Clients.Finnhub;

public class FinnhubClient(HttpClient http, IConfiguration cfg) : IFinnhubClient
{
    private readonly string _key = cfg["FINNHUB_API_KEY"] ?? "d0jprdpr01qjm8s2mecgd0jprdpr01qjm8s2med0";

    // Finnhub quote DTO
    private sealed record QuoteDto(decimal c, long v);
    // Company profile DTO
    private sealed record ProfileDto(decimal? shareOutstanding);

    public async Task<StockStatsDto?> GetStatsAsync(string symbol, CancellationToken ct = default)
    {
        symbol = symbol.ToUpperInvariant();

        var quote = await http.GetFromJsonAsync<QuoteDto>(
            $"quote?symbol={Uri.EscapeDataString(symbol)}&token={_key}", ct);
        if (quote is null) return null;

        var prof = await http.GetFromJsonAsync<ProfileDto>(
            $"stock/profile2?symbol={Uri.EscapeDataString(symbol)}&token={_key}", ct);

        long? sharesOutstanding = null;
        if (prof?.shareOutstanding is decimal millions)
        {
            // Convert millions → absolute share count, rounded to nearest whole share
            sharesOutstanding = (long)decimal.Round(millions * 1_000_000m, 0, MidpointRounding.AwayFromZero);
        }

        return new StockStatsDto(
            Symbol: symbol,
            Price: quote.c,
            TodayVolume: quote.v,
            SharesOutstanding: sharesOutstanding
        );
    }
}
