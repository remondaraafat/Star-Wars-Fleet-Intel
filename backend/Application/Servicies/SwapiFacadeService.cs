using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using Domain.Models;
using FluentValidation;
using global::Application.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
namespace Application.Servicies
{


    public class SwapiFacadeService : ISwapiFacadeService
    {
        private readonly ISwapiClient _client;
        private readonly ILogger<SwapiFacadeService> _logger;
        private readonly IValidator<Starship> _validator;
        private readonly ObjectPool<ValidationHandler> _validationPool;
        private readonly ICorrelationContextAccessor _correlationAccessor;

        public SwapiFacadeService(
            ISwapiClient client,
            ILogger<SwapiFacadeService> logger,
            IValidator<Starship> validator,
            ObjectPool<ValidationHandler> validationPool,
            ICorrelationContextAccessor correlationAccessor)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _validationPool = validationPool ?? throw new ArgumentNullException(nameof(validationPool));
            _correlationAccessor = correlationAccessor ?? throw new ArgumentNullException(nameof(correlationAccessor));
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
            if (!string.IsNullOrEmpty(search)) endpoint += $"?search={Uri.EscapeDataString(search)}";

            try
            {
                var starships = await _client.GetStarshipsAsync(endpoint, ct);
                var validationHandler = _validationPool.Get();
                try
                {
                    var validated = await validationHandler.HandleAsync(starships, ct);
                    return validated;
                }
                finally
                {
                    _validationPool.Return(validationHandler);
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
                var validationHandler = _validationPool.Get();
                try
                {
                    await validationHandler.HandleAsync(new[] { starship }, ct);
                }
                finally
                {
                    _validationPool.Return(validationHandler);
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
