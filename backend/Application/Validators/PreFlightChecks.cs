using Domain.Models;
using FluentValidation;
using System.Linq;

namespace Application.Validators
{
    public class PreFlightChecks : AbstractValidator<Starship>, IPreflightPipeline
    {
        private static readonly string[] AllowedUnknowns = { "unknown", "none", "n/a" };

        public PreFlightChecks()
        {
            RuleFor(s => s.Cost_In_Credits)
                .Must(BeValidNumberOrUnknown).WithMessage("Cost in credits must be a valid number or unknown keyword.");

            RuleFor(s => s.Max_Atmosphering_Speed)
                .Must(BeValidNumberOrUnknown).WithMessage("Max atmosphering speed must be a valid number or unknown keyword.");

            RuleFor(s => s.Hyperdrive_Rating)
                .Must(BeValidNumberOrUnknown).WithMessage("Hyperdrive rating must be a valid number or unknown keyword.");

            RuleFor(s => s.MGLT)
                .Must(BeValidNumberOrUnknown).WithMessage("MGLT must be a valid number or unknown keyword.");

            RuleFor(s => s.Cargo_Capacity)
                .Must(BeValidNumberOrUnknown).WithMessage("Cargo capacity must be a valid number or unknown keyword.");
        }

        public bool BeValidNumberOrUnknown(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;

            var normalized = value.Trim().ToLower();

            // allow keywords
            if (AllowedUnknowns.Contains(normalized))
                return true;

            // clean the value: remove commas and any non-digit characters except dot or minus
            var cleaned = new string(value.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            // try parse
            return long.TryParse(cleaned, out _) || double.TryParse(cleaned, out _);
        }
    }
}
