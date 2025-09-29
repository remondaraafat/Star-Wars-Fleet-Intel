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
            RuleFor(s => s.Name).NotEmpty().WithMessage("Starship name is required");
            RuleFor(s => s.Model).NotEmpty().WithMessage("Starship model is required");
            RuleFor(s => s.CostInCredits)
                .Must(c => c == "unknown" || decimal.TryParse(c, out _))
                .WithMessage("CostInCredits must be a valid number or 'unknown'");
            RuleFor(s => s.Url)
                .NotEmpty().WithMessage("Starship URL is required")
                .Must(u => int.TryParse(u.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var id) && id > 0)
                .WithMessage("URL must contain a valid numeric ID");
        }
    }
}
