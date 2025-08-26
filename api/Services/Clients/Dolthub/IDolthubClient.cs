using Api.Models;

namespace Api.Services.Clients.Dolthub;

public interface IDolthubClient
{
    /// <summary>
    /// Fetch tickers for a given earnings date (yyyy-MM-dd) from DoltHub.
    /// </summary>
    Task<IEnumerable<Ticker>> GetByDateAsync(DateOnly date, CancellationToken ct = default);
}
