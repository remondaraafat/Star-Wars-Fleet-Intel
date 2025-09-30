using Application.Interfaces;
using Application.ChainHandler;
using Application.Validators;
using Domain.Models;
using Microsoft.Extensions.Logging;
using CorrelationId.Abstractions;
using FluentValidation;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public class SwapiFacadeService : ISwapiFacadeService
    {
        private readonly ISwapiClient _client;
        private readonly ILogger<SwapiFacadeService> _logger;
        private readonly IValidator<Starship> _validator;
        
        private readonly ICorrelationContextAccessor _correlationAccessor;

        private readonly ObjectPool<ValidationHandler> _validationPool;

        public SwapiFacadeService(
            ISwapiClient client,
            ILogger<SwapiFacadeService> logger,
            IValidator<Starship> validator,
            ObjectPool<ValidationHandler> validationPool,
            ICorrelationContextAccessor correlationAccessor)
        {
            _client = client;
            _logger = logger;
            _validator = validator;
            _validationPool = validationPool;
            _correlationAccessor = correlationAccessor;
        }


        public async Task<IEnumerable<Starship>> GetStarshipsAsync(string? search = null, CancellationToken ct = default)
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

                // Build CoR: Sanitization -> Validation
                var sanitizer = new SanitizationHandler();
                var validatorHandler = _validationPool.Get();
                sanitizer.SetNext(validatorHandler);

                try
                {
                    var processed = await sanitizer.HandleAsync(starships, ct);
                    return processed;
                }
                finally
                {
                    _validationPool.Return(validatorHandler);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch starships");
                throw;
            }
        }

        public async Task<StarshipResponse> GetEnrichedStarshipByIdAsync(int id, CancellationToken ct = default)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Id"] = id,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });

            _logger.LogInformation("Fetching enriched starship by id: {Id}", id);

            try
            {
                var starship = await _client.GetStarshipByIdAsync(id, ct);

                // Build CoR: Sanitization -> Validation
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

                var response = new StarshipResponse
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
                    Films = new List<Film>(),
                    Pilots = new List<Person>()
                };

                foreach (var pilotUrl in starship.Pilots ?? Enumerable.Empty<string>())
                {
                    if (int.TryParse(pilotUrl.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var pilotId))
                    {
                        var person = await _client.GetPersonByIdAsync(pilotId, ct);
                        response.Pilots.ToList().Add(person);
                    }
                }

                foreach (var filmUrl in starship.Films ?? Enumerable.Empty<string>())
                {
                    if (int.TryParse(filmUrl.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var filmId))
                    {
                        var film = await _client.GetFilmByIdAsync(filmId, ct);
                        response.Films.ToList().Add(film);
                    }
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch enriched starship by id: {Id}", id);
                throw;
            }
        }
    }
}
