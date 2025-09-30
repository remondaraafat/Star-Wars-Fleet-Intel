using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Validators;
using Domain.Models;
using FluentValidation;
using FluentValidation.Results;
namespace Application.Handlers
{

    public class ValidationHandler : IStarshipListValidationHandler
    {
        private readonly IValidator<Starship> _validator;

        public ValidationHandler(IValidator<Starship> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Starship>> HandleAsync(IEnumerable<Starship> starships, CancellationToken ct = default)
        {
            var validatedStarships = new List<Starship>();
            foreach (var starship in starships)
            {
                ValidationResult result = await _validator.ValidateAsync(starship, ct);
                if (!result.IsValid)
                {
                    throw new ValidationException($"Validation failed for starship {starship.Name}: {string.Join(", ", result.Errors.Select(e => e.ErrorMessage))}");
                }
                validatedStarships.Add(starship);
            }
            return validatedStarships;
        }
    }
}
