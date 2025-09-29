using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using FluentValidation;
namespace Application.Handlers
{

    public class ValidationHandler : IStarshipListHandler
    {
        private readonly IValidator<Starship> _validator;

        public ValidationHandler(IValidator<Starship> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IEnumerable<Starship>> HandleAsync(IEnumerable<Starship> starships, CancellationToken ct = default)
        {
            foreach (var starship in starships)
            {
                var result = await _validator.ValidateAsync(starship, ct);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }
            return starships;
        }
    }
}
