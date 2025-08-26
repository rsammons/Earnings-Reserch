namespace Api.Models;

public class Ticker
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public DateOnly? ReleaseDate { get; set; }      // e.g., 2025-08-26
    public string?  ReleaseSession { get; set; }    // e.g., "Before Open", "After Close", or null
     public bool Tradable { get; set; }             // mark watchlist/tradable symbols

}
