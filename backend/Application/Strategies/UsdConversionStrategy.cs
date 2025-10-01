using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class UsdConversionStrategy : ICurrencyConversionStrategy
    {
        private const decimal Rate = 0.74m; // 1 Galactic Credit = 0.74 USD
        public string CurrencySymbol => "USD";
        public decimal Convert(decimal galacticCredits) => galacticCredits * Rate;
    }

}
