using Api.Models;

namespace Api.Services.Clients.Finnhub;

public interface IFinnhubClient
{
    Task<StockStatsDto?> GetStatsAsync(string symbol, CancellationToken ct = default);
}
