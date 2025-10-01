using Application.Validators;
using Bogus;
using Domain.DTOs;
using FluentValidation;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace StarWars.Application.Tests.Handlers
{
    public class ValidationHandlerTests
    {
        private readonly ValidationHandler _handler;
        private readonly IValidator<StarshipRequestDto> _validator;

        public ValidationHandlerTests()
        {
            _validator = new PreFlightChecks();
            _handler = new ValidationHandler(_validator);
        }

        [Fact]
        public async Task Should_Throw_When_Model_Is_Invalid()
        {
            var dtos = new List<StarshipRequestDto>
            {
                new StarshipRequestDto { Cost_In_Credits = "invalid" }
            };

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _handler.HandleAsync(dtos, CancellationToken.None));
        }

        [Fact]
        public async Task Should_Pass_When_Model_Is_Valid()
        {
            var dtos = new List<StarshipRequestDto>
            {
                new StarshipRequestDto { Cost_In_Credits = "5000" }
            };

            await _handler.HandleAsync(dtos, CancellationToken.None);
            // No exception = test passes
        }

        [Fact]
        public async Task Should_Validate_Multiple_Fake_Models()
        {
            var faker = new Faker<StarshipRequestDto>()
                .RuleFor(s => s.Cost_In_Credits, f => f.PickRandom(new[] { "unknown", "none", f.Random.Number(1, 100000).ToString() }))
                .RuleFor(s => s.Max_Atmosphering_Speed, f => f.PickRandom(new[] { "n/a", f.Random.Number(100, 2000).ToString() }));

            var dtos = faker.Generate(5);

            await _handler.HandleAsync(dtos, CancellationToken.None);
            // No exception = test passes
        }
    }
}
