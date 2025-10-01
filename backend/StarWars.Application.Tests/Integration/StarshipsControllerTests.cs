using System.Net;
using System.Net.Http.Json;
using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs;

namespace StarWars.Application.Tests.Integration
{
    public class StarshipsControllerTests : IClassFixture<WebApplicationFactory<API.Program>>
    {
        private readonly HttpClient _client;
        private readonly Faker _faker;

        public StarshipsControllerTests(WebApplicationFactory<API.Program> factory)
        {
            _client = factory.CreateClient();
            _faker = new Faker();
        }

        [Fact]
        public async Task GetStarships_WithoutSearch_Should_ReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/api/starships");

            // Assert
            response.EnsureSuccessStatusCode(); // 200
            var starships = await response.Content.ReadFromJsonAsync<List<GetStarshipsDto>>();
            Assert.NotNull(starships);
            Assert.NotEmpty(starships);
        }

        [Fact]
        public async Task GetStarships_WithSearch_Should_ReturnOk()
        {
            // Generate a fake search string
            var search = _faker.Random.Word();

            var response = await _client.GetAsync($"/api/starships?search={search}");
            response.EnsureSuccessStatusCode();

            var starships = await response.Content.ReadFromJsonAsync<List<GetStarshipsDto>>();
            Assert.NotNull(starships);
        }

        [Fact]
        public async Task GetStarshipById_ValidId_Should_ReturnOk()
        {
            int validId = 2;

            var response = await _client.GetAsync($"/api/starships/{validId}");
            response.EnsureSuccessStatusCode();

            var starship = await response.Content.ReadFromJsonAsync<GetEnrichedStarshipDto>();
            Assert.NotNull(starship);
            Assert.Equal(validId, starship.Id);
        }

        [Fact]
        public async Task GetStarshipById_InvalidId_Should_ReturnNotFound()
        {
            int invalidId = 1;

            var response = await _client.GetAsync($"/api/starships/{invalidId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetStarshipById_CancellationToken_Should_CancelRequest()
        {
            var cts = new System.Threading.CancellationTokenSource();
            cts.Cancel();

            int validId = 2;

            await Assert.ThrowsAsync<TaskCanceledException>(async () =>
            {
                await _client.GetAsync($"/api/starships/{validId}", cts.Token);
            });
        }
    }
}
