using Application.ChainHandler;
using Application.Interfaces;
using Application.Servicies;
using Application.Strategies;
using Application.Validators;
using CorrelationId.Abstractions;
using Domain.DTOs;
using Domain.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SwapiFacadeService : ISwapiFacadeService
    {
        private readonly ISwapiClient _client;
        private readonly ILogger<SwapiFacadeService> _logger;
        private readonly IValidator<StarshipRequestDto> _validator;
        private readonly ICorrelationContextAccessor _correlationAccessor;
        private readonly ObjectPool<ValidationHandler> _validationPool;
        private readonly CurrencyConverter _converter;

        private readonly IEnumerable<Domain.Decorators.IStarshipDecorator> _decorators;
        public SwapiFacadeService(
            ISwapiClient client,
            ILogger<SwapiFacadeService> logger,
            IValidator<StarshipRequestDto> validator,
            ObjectPool<ValidationHandler> validationPool,
            ICorrelationContextAccessor correlationAccessor,
            CurrencyConverter converter,
            IEnumerable<Domain.Decorators.IStarshipDecorator> decorators) 
        {
            _client = client;
            _logger = logger;
            _validator = validator;
            _validationPool = validationPool;
            _correlationAccessor = correlationAccessor;
            _converter = converter;
            _decorators = decorators ?? Enumerable.Empty<Domain.Decorators.IStarshipDecorator>();
        }


        public async Task<IEnumerable<GetStarshipsDto>> GetStarshipsAsync(string? search = null, CancellationToken ct = default)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Search"] = search ?? "none",
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });

            _logger.LogInformation("Fetching starships with search: {Search}", search);

            string endpoint = "starships/";
            if (!string.IsNullOrEmpty(search))
                endpoint += $"?search={Uri.EscapeDataString(search)}";

            try
            {
                var starships = await _client.GetStarshipsAsync(endpoint, ct);

                // Chain of Responsibility: Sanitization -> Validation
                var sanitizer = new SanitizationHandler();
                var validatorHandler = _validationPool.Get();
                sanitizer.SetNext(validatorHandler);

                IEnumerable<StarshipRequestDto> processed;
                try
                {
                    processed = await sanitizer.HandleAsync(starships, ct);
                }
                finally
                {
                    _validationPool.Return(validatorHandler);
                }

                // Map domain objects to DTOs
                var starshipDtos = processed.Select(s =>
                {
                    decimal? convertedCost = null;
                    if (decimal.TryParse(s.Cost_In_Credits, out var credits))
                    {
                        convertedCost = _converter.Convert(credits);
                    }

                    return new GetStarshipsDto
                    {
                        Name = s.Name,
                        Model = s.Model,
                        Manufacturer = s.Manufacturer,
                        CostInCredits = s.Cost_In_Credits,
                        ConvertedCost = convertedCost ?? 0,
                        CurrencySymbol = _converter.CurrencySymbol,
                        Length = s.Length,
                        Crew = s.Crew,
                        Passengers = s.Passengers,
                        MaxAtmospheringSpeed = s.Max_Atmosphering_Speed,
                        HyperdriveRating = s.Hyperdrive_Rating,
                        MGLT = s.MGLT,
                        CargoCapacity = s.Cargo_Capacity,
                        Consumables = s.Consumables,
                        Url = s.Url
                    };
                }).ToList();

                return starshipDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch starships");
                throw;
            }
        }

        public async Task<GetEnrichedStarshipDto> GetEnrichedStarshipByIdAsync(int id, CancellationToken ct = default)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["StarshipId"] = id,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });

            _logger.LogInformation("Fetching enriched starship by id: {Id}", id);

            try
            {
                var starship = await _client.GetStarshipByIdAsync(id, ct);
                // Chain of Responsibility: Sanitization -> Validation
                var sanitizer = new SanitizationHandler();
                var validatorHandler = _validationPool.Get();
                sanitizer.SetNext(validatorHandler);

                try
                {
                    await sanitizer.HandleAsync(new[] { starship }, ct);
                }
                finally
                {
                    _validationPool.Return(validatorHandler);
                }

                // Fetch pilots
                var pilotTasks = starship.Pilots.Select(url =>
                {
                    if (int.TryParse(url.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var pilotId))
                        return _client.GetPersonByIdAsync(pilotId, ct);
                    return Task.FromResult<Person?>(null);
                });
                var pilots = await Task.WhenAll(pilotTasks);

                // Fetch films
                var filmTasks = starship.Films.Select(url =>
                {
                    if (int.TryParse(url.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var filmId))
                        return _client.GetFilmByIdAsync(filmId, ct);
                    return Task.FromResult<Film?>(null);
                });
                var films = await Task.WhenAll(filmTasks);

                // Convert cost if numeric
                decimal? convertedCost = null;
                if (decimal.TryParse(starship.Cost_In_Credits, out var credits))
                {
                    convertedCost = _converter.Convert(credits);
                }

                // create enriched domain model
                var enriched = new EnrichedStarship
                {
                    Name = starship.Name,
                    Model = starship.Model,
                    Manufacturer = starship.Manufacturer,
                    Cost_In_Credits = starship.Cost_In_Credits,
                    ConvertedCost = convertedCost,
                    CurrencySymbol = _converter.CurrencySymbol,
                    Length = starship.Length,
                    Crew = starship.Crew,
                    Passengers = starship.Passengers,
                    Max_Atmosphering_Speed = starship.Max_Atmosphering_Speed,
                    Hyperdrive_Rating = starship.Hyperdrive_Rating,
                    MGLT = starship.MGLT,
                    Cargo_Capacity = starship.Cargo_Capacity,
                    Consumables = starship.Consumables,
                    Url = starship.Url,
                    ResolvedPilots = pilots.Where(p => p != null).Select(p => p!).ToList(),
                    ResolvedFilms = films.Where(f => f != null).Select(f => f!).ToList()
                };

                // Apply decorators in registration order
                foreach (var decorator in _decorators)
                {
                    enriched = decorator.Apply(enriched);
                }

                // Map enriched -> DTO
                var responseDto = new GetEnrichedStarshipDto
                {
                    Name = enriched.Name,
                    Model = enriched.Model,
                    Manufacturer = enriched.Manufacturer,
                    CostInCredits = enriched.Cost_In_Credits,
                    ConvertedCost = enriched.ConvertedCost ?? 0,
                    CurrencySymbol = enriched.CurrencySymbol,
                    Length = enriched.Length,
                    Crew = enriched.Crew,
                    Passengers = enriched.Passengers,
                    MaxAtmospheringSpeed = enriched.Max_Atmosphering_Speed,
                    HyperdriveRating = enriched.Hyperdrive_Rating,
                    MGLT = enriched.MGLT,
                    CargoCapacity = enriched.Cargo_Capacity,
                    Consumables = enriched.Consumables,
                    Url = enriched.Url,
                    ShieldBoost = enriched.ShieldBoost,
                    TargetingAccuracy = enriched.TargetingAccuracy,
                    Pilots = enriched.ResolvedPilots.Select(p => new GetPersonDto
                    {
                        Name = p.Name,
                        BirthYear = p.BirthYear,
                        Gender = p.Gender,
                        Height = p.Height,
                        Mass = p.Mass,
                        HairColor = p.HairColor,
                        EyeColor = p.EyeColor,
                        SkinColor = p.SkinColor,
                        Homeworld = p.Homeworld,
                        Created = p.Created,
                        Edited = p.Edited,
                        Url = p.Url
                    }).ToList(),
                    Films = enriched.ResolvedFilms.Select(f => new GetFilmDto
                    {
                        Title = f.Title,
                        EpisodeId = f.EpisodeId,
                        OpeningCrawl = f.OpeningCrawl,
                        Director = f.Director,
                        Producer = f.Producer,
                        ReleaseDate = f.ReleaseDate,
                        Created = f.Created,
                        Edited = f.Edited,
                        Url = f.Url
                    }).ToList()
                };


                return responseDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch enriched starship by id: {Id}", id);
                throw;
            }
        }
    }
}
