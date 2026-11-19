using Microsoft.EntityFrameworkCore;
using WeatherDesk.Core.Abstractions;

namespace WeatherDesk.Web.Data;

public class SqliteSearchStore : IRecentSearchStore
{
    private readonly DbContextOptions<HistoryDbContext> _options;

    public SqliteSearchStore(string connectionString)
    {
        _options = new DbContextOptionsBuilder<HistoryDbContext>()
            .UseSqlite(connectionString)
            .Options;
    }

    public void EnsureCreated()
    {
        using var db = new HistoryDbContext(_options);
        db.Database.EnsureCreated();
    }

    public void Add(string city)
    {
        var db = new HistoryDbContext(_options);
        db.Searches.Add(new SearchEntry { City = city, SearchedAt = DateTime.UtcNow });
        db.SaveChanges();
    }

    public IReadOnlyList<string> GetAll()
    {
        var db = new HistoryDbContext(_options);
        return db.Searches.OrderBy(s => s.Id).Select(s => s.City).ToList();
    }
}
