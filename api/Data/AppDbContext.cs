using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticker> Tickers => Set<Ticker>();
    public DbSet<StockStatsSnapshot> StockStatsSnapshots => Set<StockStatsSnapshot>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Ticker>().HasIndex(t => new { t.Symbol, t.ReleaseDate }).IsUnique();
        b.Entity<StockStatsSnapshot>().HasIndex(s => new { s.TickerId, s.AsOfDate }).IsUnique();

        b.Entity<Ticker>().HasKey(t => t.Id);
        b.Entity<StockStatsSnapshot>().HasKey(t => t.Id);
    }
}
