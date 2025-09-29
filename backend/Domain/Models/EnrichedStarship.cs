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
        public decimal? ConvertedCost { get; set; }  // From Strategy
        public int ShieldBoost { get; set; }        // From Decorator
        public int TargetingAccuracy { get; set; }  // From Decorator
        public IEnumerable<Film> ResolvedFilms { get; set; }
        public IEnumerable<Person> ResolvedPilots { get; set; }
    }
}
