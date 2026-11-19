# WeatherDesk

A small ASP.NET Core 8 app that shows the current weather for a city, using the free [Open-Meteo](https://open-meteo.com/) API (no key needed). It runs on your own computer.

## Requirements
- .NET 8 SDK (a newer SDK also works), Git, and internet access (for NuGet and Open-Meteo).

## Run it
```
dotnet build WeatherDesk.sln
dotnet test WeatherDesk.sln
dotnet run --project src/WeatherDesk.Web
```
Open http://localhost:5080. The page needs two JavaScript libraries that are checked in under `src/WeatherDesk.Web/wwwroot/lib`. If they are missing, run `scripts/Add-VendoredLibs.ps1` once.

## Endpoints
| Endpoint | What it does |
|---|---|
| `GET /api/weather?city=Victoria&country=CA` | Current weather for one city. `country` (two letters) is optional. |
| `GET /api/compare?cities=Victoria,Toronto` | Compares several cities. |
| `GET /api/recent` | The cities looked up recently. |
| `GET /health` | `{"status":"ok"}` |

## Layout
- `src/WeatherDesk.Core`: logic with no web or database dependency (models, rules, services, JSON parsing).
- `src/WeatherDesk.Web`: the web host, the Open-Meteo client, the SQLite search history and the page.
- `tests/WeatherDesk.Tests`: xUnit tests for `WeatherDesk.Core`.
- Package versions are central, in `Directory.Packages.props`.

## Behaviour reference
This is how the app is meant to behave.

**Temperature.** Celsius is shown to one decimal place. Fahrenheit is `C x 9/5 + 32`, also to one decimal place.

**Wind** (km/h) is described as: Calm below 2; Light from 2 up to (not including) 20; Moderate from 20 up to 40; Strong from 40 up to 62; Storm from 62 up. The page shows wind in km/h.

**How it feels** (degrees C): Bitterly cold below -10; Freezing from -10 up to 0; Cold from 0 up to 10; Cool from 10 up to 18; Pleasant from 18 up to 25; Warm from 25 up to 32; Hot from 32 up.

**City names.** Leading and trailing spaces are removed and repeated inner spaces become one. A name must be 2 to 60 characters and may contain only letters (any language), spaces, hyphens, apostrophes and full stops. Anything else is rejected with a clear message.

**Lookup responses.** A good lookup returns the weather. An invalid name returns 400 with `{"error": "..."}`. A city that does not exist returns 404 with `{"error": "..."}`. If the weather service cannot be reached the app returns 502 with a short, friendly message. Error responses never contain stack traces or file paths.

**Compare.** Accepts 1 to 5 different cities (the same city in a different case counts once, blank entries are ignored). It returns the warmest city, the coldest city, the average temperature in degrees C to one decimal place, and how many cities were compared. More than 5 cities, or no usable city, returns 400 with a clear message.

**Recent searches.** The last 5 different cities (the same city in a different case counts once), newest first.
