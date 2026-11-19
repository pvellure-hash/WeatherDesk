using Microsoft.EntityFrameworkCore;

namespace WeatherDesk.Web.Data;

public class SearchEntry
{
    public int Id { get; set; }

    public string City { get; set; } = string.Empty;

    public DateTime SearchedAt { get; set; }
}

public class HistoryDbContext : DbContext
{
    public HistoryDbContext(DbContextOptions<HistoryDbContext> options) : base(options)
    {
    }

    public DbSet<SearchEntry> Searches => Set<SearchEntry>();
}
