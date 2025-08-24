using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Ticker> Tickers => Set<Ticker>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Ticker>().HasIndex(t => t.Symbol).IsUnique();
    }
}
