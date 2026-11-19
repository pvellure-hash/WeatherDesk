using WeatherDesk.Core.Abstractions;

namespace WeatherDesk.Core.Services;

public sealed class RecentSearchService
{
    private readonly IRecentSearchStore _store;

    public RecentSearchService(IRecentSearchStore store)
    {
        _store = store;
    }

    public void Record(string city) => _store.Add(city);

    public IReadOnlyList<string> GetRecent() => _store.GetAll();
}
