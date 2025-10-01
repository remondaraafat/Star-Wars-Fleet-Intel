using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class EnrichedStarship : Starship
    {
        // Strategy output
        public decimal? ConvertedCost { get; set; } = null;
        public string? CurrencySymbol { get; set; } = null;

        // Decorator fields (defaults: 0)
        public int ShieldBoost { get; set; } = 0;
        public int TargetingAccuracy { get; set; } = 0;

        // Resolved references
        public IEnumerable<Film> ResolvedFilms { get; set; } = new List<Film>();
        public IEnumerable<Person> ResolvedPilots { get; set; } = new List<Person>();
    }
}
