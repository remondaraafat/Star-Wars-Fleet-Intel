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
            RuleFor(s => s.Cost_In_Credits)
                .Must(c => c == "unknown" || decimal.TryParse(c.Replace(",", ""), out _))
                .WithMessage("Cost in credits must be a valid number or 'unknown'.");

            RuleFor(s => s.Length)
                 .Must(l => l == "unknown" || double.TryParse(l.Replace(",", ""), out _))
                 .WithMessage("Length must be a valid number or 'unknown'.");
            RuleFor(s => s.Crew)
                .NotEmpty().WithMessage("Crew cannot be empty.");
            RuleFor(s => s.Passengers)
                .NotEmpty().WithMessage("Passengers cannot be empty.");
            RuleFor(s => s.Max_Atmosphering_Speed)
                .Must(s =>
                    string.IsNullOrEmpty(s) ||
                    s == "n/a" ||
                    s == "unknown" ||
                    s == "none" ||
                    int.TryParse(
                        new string(s.Where(char.IsDigit).ToArray()), // keep only digits
                        out _
                    )
                )
                .WithMessage("Max atmosphering speed must be a valid number, 'n/a', 'unknown', or 'none'.");


            RuleFor(s => s.Hyperdrive_Rating)
                .Must(h =>
                    string.IsNullOrEmpty(h) ||
                    h == "unknown" ||
                    double.TryParse(h, out _)
                )
                .WithMessage("Hyperdrive rating must be a valid number or 'unknown'.");

            RuleFor(s => s.MGLT)
                .Must(m =>
                    string.IsNullOrEmpty(m) ||
                    m == "unknown" ||
                    int.TryParse(m, out _)
                )
                .WithMessage("MGLT must be a valid number or 'unknown'.");

            RuleFor(s => s.Cargo_Capacity)
                .Must(c =>
                    string.IsNullOrEmpty(c) ||
                    c == "unknown" ||
                    double.TryParse(c.Replace(",", ""), out _)
                )
                .WithMessage("Cargo capacity must be a valid number or 'unknown'.");
            RuleFor(s => s.Consumables)
                .NotEmpty().WithMessage("Consumables cannot be empty.");
        }
    }
}
