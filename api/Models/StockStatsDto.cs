namespace Api.Models;

public record StockStatsDto(
    string Symbol,
    decimal Price,
    long TodayVolume,
    long? SharesOutstanding
);
