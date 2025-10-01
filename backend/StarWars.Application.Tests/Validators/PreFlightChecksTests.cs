using Application.Validators;
using Bogus;
using Domain.DTOs;
using FluentValidation.TestHelper;
using Xunit;

namespace StarWars.Application.Tests.Validators
{
    public class PreFlightChecksTests
    {
        private readonly PreFlightChecks _validator;

        public PreFlightChecksTests()
        {
            _validator = new PreFlightChecks();
        }

        [Fact]
        public void Should_Have_Error_When_CostInCredits_Is_Invalid()
        {
            var model = new StarshipRequestDto { Cost_In_Credits = "invalid" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(s => s.Cost_In_Credits);
        }

        [Fact]
        public void Should_Validate_All_Allowed_Keywords()
        {
            var keywords = new[] { "unknown", "none", "n/a" };
            foreach (var kw in keywords)
            {
                var model = new StarshipRequestDto
                {
                    Cost_In_Credits = kw,
                    Max_Atmosphering_Speed = kw,
                    Hyperdrive_Rating = kw,
                    MGLT = kw,
                    Cargo_Capacity = kw
                };
                var result = _validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(x => x.Cost_In_Credits);
                result.ShouldNotHaveValidationErrorFor(x => x.Max_Atmosphering_Speed);
                result.ShouldNotHaveValidationErrorFor(x => x.Hyperdrive_Rating);
                result.ShouldNotHaveValidationErrorFor(x => x.MGLT);
                result.ShouldNotHaveValidationErrorFor(x => x.Cargo_Capacity);
            }
        }

        [Fact]
        public void Should_Validate_Model_With_Bogus()
        {
            var faker = new Faker<StarshipRequestDto>()
                .RuleFor(s => s.Cost_In_Credits, f => f.Random.Number(1000, 100000).ToString())
                .RuleFor(s => s.Max_Atmosphering_Speed, f => f.Random.Number(100, 2000).ToString())
                .RuleFor(s => s.Hyperdrive_Rating, f => f.Random.Double(0.5, 5.0).ToString("0.0"))
                .RuleFor(s => s.MGLT, f => f.Random.Number(10, 1000).ToString())
                .RuleFor(s => s.Cargo_Capacity, f => f.Random.Number(1000, 1000000).ToString());

            var model = faker.Generate();
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(x => x.Cost_In_Credits);
            result.ShouldNotHaveValidationErrorFor(x => x.Max_Atmosphering_Speed);
            result.ShouldNotHaveValidationErrorFor(x => x.Hyperdrive_Rating);
            result.ShouldNotHaveValidationErrorFor(x => x.MGLT);
            result.ShouldNotHaveValidationErrorFor(x => x.Cargo_Capacity);
        }

        [Fact]
        public void Should_Validate_Multiple_Fake_Models()
        {
            var faker = new Faker<StarshipRequestDto>()
                .RuleFor(s => s.Cost_In_Credits, f => f.PickRandom(new[] { "unknown", "none", f.Random.Number(1, 100000).ToString() }))
                .RuleFor(s => s.Max_Atmosphering_Speed, f => f.PickRandom(new[] { "n/a", f.Random.Number(100, 2000).ToString() }))
                .RuleFor(s => s.Hyperdrive_Rating, f => f.PickRandom(new[] { "unknown", f.Random.Double(0.5, 5.0).ToString("0.0") }))
                .RuleFor(s => s.MGLT, f => f.PickRandom(new[] { "n/a", f.Random.Number(10, 1000).ToString() }))
                .RuleFor(s => s.Cargo_Capacity, f => f.PickRandom(new[] { "none", f.Random.Number(1000, 1000000).ToString() }));

            var models = faker.Generate(10);

            foreach (var model in models)
            {
                var result = _validator.TestValidate(model);
                result.ShouldNotHaveValidationErrorFor(x => x.Cost_In_Credits);
                result.ShouldNotHaveValidationErrorFor(x => x.Max_Atmosphering_Speed);
                result.ShouldNotHaveValidationErrorFor(x => x.Hyperdrive_Rating);
                result.ShouldNotHaveValidationErrorFor(x => x.MGLT);
                result.ShouldNotHaveValidationErrorFor(x => x.Cargo_Capacity);
            }
        }
    }
}
