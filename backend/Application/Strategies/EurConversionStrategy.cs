using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class EurConversionStrategy : ICurrencyConversionStrategy
    {
        private const decimal Rate = 0.68m; // Example rate
        public string CurrencySymbol => "EUR";
        public decimal Convert(decimal galacticCredits) => galacticCredits * Rate;
    }
}
