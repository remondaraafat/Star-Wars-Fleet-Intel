using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{

    public class StarshipValidator : AbstractValidator<Starship>
    {
        public StarshipValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Starship name cannot be empty.");
            RuleFor(s => s.Model)
                .NotEmpty().WithMessage("Starship model cannot be empty.");
            RuleFor(s => s.Manufacturer)
                .NotEmpty().WithMessage("Starship manufacturer cannot be empty.");
            RuleFor(s => s.CostInCredits)
                .Must(c => c == "unknown" || decimal.TryParse(c, out _))
                .WithMessage("Cost in credits must be a valid number or 'unknown'.");
            RuleFor(s => s.Length)
                .Must(l => double.TryParse(l, out _))
                .WithMessage("Length must be a valid number.");
            RuleFor(s => s.Crew)
                .NotEmpty().WithMessage("Crew cannot be empty.");
            RuleFor(s => s.Passengers)
                .NotEmpty().WithMessage("Passengers cannot be empty.");
            RuleFor(s => s.MaxAtmospheringSpeed)
                .Must(s => s == "n/a" || int.TryParse(s, out _))
                .WithMessage("Max atmosphering speed must be a valid number or 'n/a'.");
            RuleFor(s => s.HyperdriveRating)
                .Must(h => string.IsNullOrEmpty(h) || double.TryParse(h, out _))
                .WithMessage("Hyperdrive rating must be a valid number or empty.");
            RuleFor(s => s.MGLT)
                .Must(m => string.IsNullOrEmpty(m) || int.TryParse(m, out _))
                .WithMessage("MGLT must be a valid number or empty.");
            RuleFor(s => s.CargoCapacity)
                .Must(c => string.IsNullOrEmpty(c) || double.TryParse(c, out _))
                .WithMessage("Cargo capacity must be a valid number or empty.");
            RuleFor(s => s.Consumables)
                .NotEmpty().WithMessage("Consumables cannot be empty.");
        }
    }
}
