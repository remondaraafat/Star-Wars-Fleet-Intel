using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Starship;
namespace Infrastructure.Swapi
{
    // Infrastructure/Swapi/SwapiClient.cs
    public class SwapiClient
    {
        private readonly HttpClient _http;
        public SwapiClient(HttpClient http) => _http = http;

        public async Task<SwapiStarshipDto> GetStarshipByIdAsync(int id, CancellationToken ct)
        {
            var resp = await _http.GetAsync($"starships/{id}/", ct);
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadFromJsonAsync<SwapiStarshipDto>(cancellationToken: ct);
            return json!;
        }
        // add other calls (search, pagination) as needed
    }

}
