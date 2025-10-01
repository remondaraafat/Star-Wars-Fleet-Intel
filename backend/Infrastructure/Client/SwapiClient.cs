using Application.Interfaces;
using CorrelationId.Abstractions;
using Domain.DTOs;
using Domain.Exceptions;
using Domain.Models;
using global::Infrastructure.SwapiProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Client
{


    public class SwapiClient : ISwapiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SwapiClient> _logger;
        private readonly bool _useFakeData;
        private readonly IFakeSwapiProvider _fakeProvider;
        private readonly ICorrelationContextAccessor _correlationAccessor;

        public SwapiClient(
                            HttpClient httpClient,
                            IConfiguration config,
                            ILogger<SwapiClient> logger,
                            IFakeSwapiProvider fakeProvider,
                            ICorrelationContextAccessor correlationAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _useFakeData = config.GetValue<bool>("SwapiClient:UseFakeData");
            _fakeProvider = fakeProvider;
            _correlationAccessor = correlationAccessor;
        }


        public async Task<IEnumerable<StarshipRequestDto>> GetStarshipsAsync(string endpoint, CancellationToken ct = default)
        {
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Endpoint"] = endpoint,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });
            _logger.LogInformation("Fetching SWAPI endpoint: {Endpoint}", endpoint);

            if (_useFakeData)
            {
                _logger.LogDebug("Using fake data for {Endpoint}", endpoint);
                return await _fakeProvider.GenerateFakeStarshipsAsync(null, ct);
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Correlation-Id");
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", _correlationAccessor.CorrelationContext.CorrelationId);

                
                var response = await _httpClient.GetAsync(endpoint, ct);
                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException($"SWAPI endpoint not found: {endpoint}");
                    }
                    // for other non-success
                    var body = await response.Content.ReadAsStringAsync(ct);
                    throw new HttpRequestException($"External API returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
                }
                var content = await response.Content.ReadAsStringAsync(ct);

                var result = JsonSerializer.Deserialize<SwapiListResponse<StarshipRequestDto>>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result?.Results ?? Enumerable.Empty<StarshipRequestDto>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<StarshipRequestDto> GetStarshipByIdAsync(int id, CancellationToken ct = default)
        {
            var endpoint = $"starships/{id}/";
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Endpoint"] = endpoint,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });
            _logger.LogInformation("Fetching SWAPI endpoint: {Endpoint}", endpoint);

            if (_useFakeData)
            {
                _logger.LogDebug("Using fake data for {Endpoint}", endpoint);
                return await _fakeProvider.GenerateFakeStarshipAsync(id, ct);
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Correlation-Id");
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", _correlationAccessor.CorrelationContext.CorrelationId);

                var response = await _httpClient.GetAsync(endpoint, ct);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new NotFoundException($"Starship with id {id} not found.");
                    }
                    var body = await response.Content.ReadAsStringAsync(ct);
                    throw new HttpRequestException($"External API returned {(int)response.StatusCode} {response.ReasonPhrase}: {body}");
                }

                var content = await response.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<StarshipRequestDto>(content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (result == null)
                    throw new NotFoundException($"Starship with id {id} not found.");

                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Endpoint}", endpoint);
                throw;
            }
        }


        public async Task<Person> GetPersonByIdAsync(int id, CancellationToken ct = default)
        {
            var endpoint = $"people/{id}/";
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Endpoint"] = endpoint,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });
            _logger.LogInformation("Fetching SWAPI endpoint: {Endpoint}", endpoint);

            if (_useFakeData)
            {
                _logger.LogDebug("Using fake data for {Endpoint}", endpoint);
                return await _fakeProvider.GenerateFakePersonAsync(id, ct);
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Correlation-Id");
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", _correlationAccessor.CorrelationContext.CorrelationId);
                var response = await _httpClient.GetAsync(endpoint, ct);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<Person>(content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result ?? throw new InvalidOperationException("Person not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<Film> GetFilmByIdAsync(int id, CancellationToken ct = default)
        {
            var endpoint = $"films/{id}/";
            using var scope = _logger.BeginScope(new Dictionary<string, object>
            {
                ["RequestId"] = Guid.NewGuid(),
                ["Endpoint"] = endpoint,
                ["CorrelationId"] = _correlationAccessor.CorrelationContext.CorrelationId
            });
            _logger.LogInformation("Fetching SWAPI endpoint: {Endpoint}", endpoint);

            if (_useFakeData)
            {
                _logger.LogDebug("Using fake data for {Endpoint}", endpoint);
                return await _fakeProvider.GenerateFakeFilmAsync(id, ct);
            }

            try
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Correlation-Id");
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-Id", _correlationAccessor.CorrelationContext.CorrelationId);

                var response = await _httpClient.GetAsync(endpoint, ct);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(ct);

                //ase-insensitive matching
                var result = JsonSerializer.Deserialize<Film>(content,
    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });


                return result ?? throw new InvalidOperationException("Film not found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Endpoint}", endpoint);
                throw;
            }
        }

    }
}
