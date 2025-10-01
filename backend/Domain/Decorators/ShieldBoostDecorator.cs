using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Domain.Decorators
{
    public class ShieldBoostDecorator : IStarshipDecorator
    {
        private readonly int _boostAmount;
        private readonly ILogger<ShieldBoostDecorator> _logger;
        public ShieldBoostDecorator(ILogger<ShieldBoostDecorator> logger)
        {
            _boostAmount = 50; // default boost
            _logger = logger;
        }

        public EnrichedStarship Apply(EnrichedStarship starship)
        {
            if (starship == null) return starship;

            starship.ShieldBoost += _boostAmount;

            _logger.LogDebug("Applying ShieldBoost {Boost} to {Starship}", _boostAmount, starship?.Name);

            return starship;
        }
    }
}
