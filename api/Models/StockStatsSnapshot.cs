// Models/StockStatsSnapshot.cs
namespace Api.Models;

public class StockStatsSnapshot
{
    public int Id { get; set; }
    public int TickerId { get; set; }                // FK → Ticker
    public Ticker Ticker { get; set; } = default!;

    public DateOnly AsOfDate { get; set; }           // e.g., trading day
    public decimal Price { get; set; }
    public long AverageDailyVolume { get; set; }
    public long? FloatShares { get; set; }           // can be null
}
