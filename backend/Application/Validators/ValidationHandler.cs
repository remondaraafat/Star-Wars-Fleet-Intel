using Domain.Models;
using FluentValidation;
using System.Linq;
using System.Globalization;
using Application.ChainHandler;

namespace Application.Validators
{
    public class ValidationHandler : StarshipHandlerBase
    {
        private readonly IValidator<Starship> _validator;

        public ValidationHandler(IValidator<Starship> validator)
        {
            _validator = validator;
        }

        public override async Task<IEnumerable<Starship>> HandleAsync(IEnumerable<Starship> starships, CancellationToken ct)
        {
            foreach (var ship in starships)
            {
                var result = await _validator.ValidateAsync(ship, ct);
                if (!result.IsValid)
                    throw new ValidationException($"Validation failed for {ship.Name}: {string.Join(", ", result.Errors.Select(e => e.ErrorMessage))}");
            }

            return await base.HandleAsync(starships, ct);
        }
    }


}
