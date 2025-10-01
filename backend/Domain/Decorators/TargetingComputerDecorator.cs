using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Decorators
{
    public class TargetingComputerDecorator : IStarshipDecorator
    {
        private readonly int _accuracyBonus;
        private readonly ILogger<TargetingComputerDecorator> _logger;
        public TargetingComputerDecorator(ILogger<TargetingComputerDecorator> logger)
        {
            _accuracyBonus = 10; 
            _logger = logger;
        }

        public EnrichedStarship Apply(EnrichedStarship starship)
        {
            if (starship == null) return starship;

            starship.TargetingAccuracy += _accuracyBonus;

            _logger.LogDebug("Applying Targeting Computer {AccuracyBonus} to {Starship}", _accuracyBonus, starship?.Name);

            return starship;
        }
    }
}
