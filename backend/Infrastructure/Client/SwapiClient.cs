using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorrelationId.Abstractions;
using Domain.Models;
using global::Infrastructure.SwapiProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Client
{


    public class SwapiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SwapiClient> _logger;
        private readonly bool _useFakeData;
        private readonly IFakeSwapiProvider _fakeProvider;
        private readonly ICorrelationContextAccessor _correlationAccessor;

        public SwapiClient(IHttpClientFactory factory, IConfiguration config, ILogger<SwapiClient> logger, IFakeSwapiProvider fakeProvider, ICorrelationContextAccessor correlationAccessor)
        {
            _httpClient = factory.CreateClient(nameof(SwapiClient));
            _logger = logger;
            _useFakeData = config.GetValue<bool>("SwapiClient:UseFakeData");
            _fakeProvider = fakeProvider;
            _correlationAccessor = correlationAccessor;
        }

        public async Task<IEnumerable<Starship>> GetStarshipsAsync(string endpoint, CancellationToken ct = default)
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
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<Starship[]>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return result ?? Enumerable.Empty<Starship>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<Starship> GetStarshipByIdAsync(int id, CancellationToken ct = default)
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
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync(ct);
                var result = JsonSerializer.Deserialize<Starship>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                return result ?? throw new InvalidOperationException("Starship not found");
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
                var result = JsonSerializer.Deserialize<Person>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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
                var result = JsonSerializer.Deserialize<Film>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
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
