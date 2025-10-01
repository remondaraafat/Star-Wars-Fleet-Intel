using Application.ChainHandler;
using Application.Interfaces;
using Application.Servicies;
using Application.Strategies;
using Application.Validators;
using CorrelationId.Abstractions;
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
        private readonly IValidator<Starship> _validator;
        private readonly ICorrelationContextAccessor _correlationAccessor;
        private readonly ObjectPool<ValidationHandler> _validationPool;
        private readonly CurrencyConverter _converter;

        public SwapiFacadeService(
            ISwapiClient client,
            ILogger<SwapiFacadeService> logger,
            IValidator<Starship> validator,
            ObjectPool<ValidationHandler> validationPool,
            ICorrelationContextAccessor correlationAccessor,
            CurrencyConverter converter)
        {
            _client = client;
            _logger = logger;
            _validator = validator;
            _validationPool = validationPool;
            _correlationAccessor = correlationAccessor;
            _converter = converter;
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

                IEnumerable<Starship> processed;
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

        public async Task<EnrichedStarshipResponseDto> GetEnrichedStarshipByIdAsync(int id, CancellationToken ct = default)
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

                // Fetch pilots in parallel
                var pilotTasks = (starship.Pilots ?? Enumerable.Empty<string>())
                    .Select(url =>
                    {
                        if (int.TryParse(url.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var pilotId))
                            return _client.GetPersonByIdAsync(pilotId, ct);
                        return Task.FromResult<Person?>(null);
                    });

                var pilots = await Task.WhenAll(pilotTasks);

                // Fetch films in parallel
                var filmTasks = (starship.Films ?? Enumerable.Empty<string>())
                    .Select(url =>
                    {
                        if (int.TryParse(url.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var filmId))
                            return _client.GetFilmByIdAsync(filmId, ct);
                        return Task.FromResult<Film?>(null);
                    });

                var films = await Task.WhenAll(filmTasks);

                
                

                // Map to DTO
                var responseDto = new EnrichedStarshipResponseDto
                {
                    Name = starship.Name,
                    Model = starship.Model,
                    Manufacturer = starship.Manufacturer,
                    CostInCredits = starship.Cost_In_Credits,
                    
                    Length = starship.Length,
                    Crew = starship.Crew,
                    Passengers = starship.Passengers,
                    MaxAtmospheringSpeed = starship.Max_Atmosphering_Speed,
                    HyperdriveRating = starship.Hyperdrive_Rating,
                    MGLT = starship.MGLT,
                    CargoCapacity = starship.Cargo_Capacity,
                    Consumables = starship.Consumables,
                    Pilots = pilots.Where(p => p != null)
                                   .Select(p => new PersonDto { Name = p!.Name })
                                   .ToList(),
                    Films = films.Where(f => f != null)
                                 .Select(f => new FilmDto { Title = f!.Title })
                                 .ToList()
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
