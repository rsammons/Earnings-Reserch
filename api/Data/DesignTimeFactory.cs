using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data;
public class DesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        try
        {
            var cwd = Directory.GetCurrentDirectory();
            var apiEnv = Path.Combine(cwd, ".env");
            if (File.Exists(apiEnv)) Env.Load(apiEnv);
            var parent = Directory.GetParent(cwd)?.FullName;
            var rootEnv = parent is null ? null : Path.Combine(parent, ".env");
            if (rootEnv is not null && File.Exists(rootEnv)) Env.Load(rootEnv);
        } catch { }

        var cs = Environment.GetEnvironmentVariable("CONNECTION_STR")
                 ?? "Host=localhost;Port=5432;Database=er_db;Username=er_user;Password=er_pass;SSL Mode=Disable;Trust Server Certificate=true";

        var opts = new DbContextOptionsBuilder<AppDbContext>().UseNpgsql(cs).Options;
        return new AppDbContext(opts);
    }
}
