using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ServiceStack.Svg;
using Bogus;
using Domain.Models;
using Person = Domain.Models.Person;
using Domain.DTOs;
namespace Infrastructure.SwapiProvider
{
    public class FakeSwapiProvider : IFakeSwapiProvider
    {
        public async Task<IEnumerable<StarshipRequestDto>> GenerateFakeStarshipsAsync(string? search, CancellationToken ct = default)
        {
            var faker = new Faker<StarshipRequestDto>()
                .RuleFor(s => s.Name, f => f.Vehicle.Model())
                .RuleFor(s => s.Model, f => f.Vehicle.Type())
                .RuleFor(s => s.Manufacturer, f => f.Company.CompanyName())
                .RuleFor(s => s.Cargo_Capacity, f => f.Random.Bool() ? f.Finance.Amount(1000, 1000000).ToString() : "unknown")
                .RuleFor(s => s.Length, f => f.Random.Double(10, 1000).ToString("F2"))
                .RuleFor(s => s.Max_Atmosphering_Speed, f => f.Random.Number(500, 1500).ToString())
                .RuleFor(s => s.Crew, f => f.Random.Number(1, 100).ToString())
                .RuleFor(s => s.Passengers, f => f.Random.Number(0, 500).ToString())
                .RuleFor(s => s.Cargo_Capacity, f => f.Random.Double(1000, 1000000).ToString("F2"))
                .RuleFor(s => s.Consumables, f => f.PickRandom("1 week", "1 month", "1 year"))
                .RuleFor(s => s.Hyperdrive_Rating, f => f.Random.Double(1, 5).ToString("F1"))
                .RuleFor(s => s.MGLT, f => f.Random.Number(50, 150).ToString())
                .RuleFor(s => s.Pilots, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/people/{f.Random.Number(1, 100)}/"))
                .RuleFor(s => s.Films, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/films/{f.Random.Number(1, 7)}/"))
                .RuleFor(s => s.Url, f => $"https://swapi.dev/api/starships/{f.IndexGlobal + 1}/");

            var starships = faker.Generate(10);
            if (!string.IsNullOrEmpty(search))
                starships = starships.Where(s => s.Name.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            return await Task.FromResult(starships);
        }

        public async Task<StarshipRequestDto> GenerateFakeStarshipAsync(int id, CancellationToken ct = default)
        {
            var faker = new Faker<StarshipRequestDto>()
                .RuleFor(s => s.Name, f => f.Vehicle.Model())
                .RuleFor(s => s.Model, f => f.Vehicle.Type())
                .RuleFor(s => s.Manufacturer, f => f.Company.CompanyName())
                .RuleFor(s => s.Cost_In_Credits, f => f.Random.Bool() ? f.Finance.Amount(1000, 1000000).ToString() : "unknown")
                .RuleFor(s => s.Length, f => f.Random.Double(10, 1000).ToString("F2"))
                .RuleFor(s => s.Max_Atmosphering_Speed, f => f.Random.Number(500, 1500).ToString())
                .RuleFor(s => s.Crew, f => f.Random.Number(1, 100).ToString())
                .RuleFor(s => s.Passengers, f => f.Random.Number(0, 500).ToString())
                .RuleFor(s => s.Cargo_Capacity, f => f.Random.Double(1000, 1000000).ToString("F2"))
                .RuleFor(s => s.Consumables, f => f.PickRandom("1 week", "1 month", "1 year"))
                .RuleFor(s => s.Hyperdrive_Rating, f => f.Random.Double(1, 5).ToString("F1"))
                .RuleFor(s => s.MGLT, f => f.Random.Number(50, 150).ToString())
                .RuleFor(s => s.Pilots, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/people/{f.Random.Number(1, 100)}/"))
                .RuleFor(s => s.Films, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/films/{f.Random.Number(1, 7)}/"))
                .RuleFor(s => s.Url, f => $"https://swapi.dev/api/starships/{id}/");

            return await Task.FromResult(faker.Generate());
        }

        public async Task<Person> GenerateFakePersonAsync(int id, CancellationToken ct = default)
        {
            var faker = new Faker<Person>()
                .RuleFor(p => p.Name, f => f.Name.FullName())
                .RuleFor(p => p.Height, f => f.Random.Number(150, 200).ToString())
                .RuleFor(p => p.Mass, f => f.Random.Double(50, 150).ToString("F2"))
                .RuleFor(p => p.HairColor, f => f.PickRandom("brown", "black", "blonde", "none"))
                .RuleFor(p => p.SkinColor, f => f.PickRandom("light", "dark", "green"))
                .RuleFor(p => p.EyeColor, f => f.PickRandom("blue", "brown", "green"))
                .RuleFor(p => p.BirthYear, f => f.PickRandom("19BBY", "57BBY", "unknown"))
                .RuleFor(p => p.Gender, f => f.PickRandom("male", "female", "n/a"))
                .RuleFor(p => p.Homeworld, f => $"https://swapi.dev/api/planets/{f.Random.Number(1, 60)}/")
                .RuleFor(p => p.Films, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/films/{f.Random.Number(1, 7)}/"))
                .RuleFor(p => p.Species, f => f.Make(f.Random.Number(0, 2), () => $"https://swapi.dev/api/species/{f.Random.Number(1, 37)}/"))
                .RuleFor(p => p.Vehicles, f => f.Make(f.Random.Number(0, 2), () => $"https://swapi.dev/api/vehicles/{f.Random.Number(1, 50)}/"))
                .RuleFor(p => p.Starships, f => f.Make(f.Random.Number(0, 2), () => $"https://swapi.dev/api/starships/{f.Random.Number(1, 50)}/"))
                .RuleFor(p => p.Url, f => $"https://swapi.dev/api/people/{id}/");

            return await Task.FromResult(faker.Generate());
        }

        public async Task<Film> GenerateFakeFilmAsync(int id, CancellationToken ct = default)
        {
            var faker = new Faker<Film>()
                .RuleFor(f => f.Title, f => f.Lorem.Sentence(3))
                .RuleFor(f => f.EpisodeId, f => f.Random.Number(1, 9))
                .RuleFor(f => f.OpeningCrawl, f => f.Lorem.Paragraph())
                .RuleFor(f => f.Director, f => f.Name.FullName())
                .RuleFor(f => f.Producer, f => f.Name.FullName())
                .RuleFor(f => f.ReleaseDate, f => f.Date.Past(40).ToString("yyyy-MM-dd"))
                .RuleFor(f => f.Characters, f => f.Make(f.Random.Number(0, 5), () => $"https://swapi.dev/api/people/{f.Random.Number(1, 100)}/"))
                .RuleFor(f => f.Planets, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/planets/{f.Random.Number(1, 60)}/"))
                .RuleFor(f => f.Starships, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/starships/{f.Random.Number(1, 50)}/"))
                .RuleFor(f => f.Vehicles, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/vehicles/{f.Random.Number(1, 50)}/"))
                .RuleFor(f => f.Species, f => f.Make(f.Random.Number(0, 3), () => $"https://swapi.dev/api/species/{f.Random.Number(1, 37)}/"))
                .RuleFor(f => f.Url, f => $"https://swapi.dev/api/films/{id}/");

            return await Task.FromResult(faker.Generate());
        }
    }
}
