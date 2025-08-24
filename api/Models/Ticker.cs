namespace Api.Models;

public class Ticker
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string EarningRelease { get; set; } = string.Empty;
     public bool Tradable { get; set; }             // mark watchlist/tradable symbols

}
