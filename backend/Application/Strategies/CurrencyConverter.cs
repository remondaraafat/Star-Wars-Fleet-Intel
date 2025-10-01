using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{
    public class CurrencyConverter
    {
        private ICurrencyConversionStrategy _strategy;

        public CurrencyConverter(ICurrencyConversionStrategy strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(ICurrencyConversionStrategy strategy) => _strategy = strategy;

        public decimal Convert(decimal credits) => _strategy.Convert(credits);

        public string CurrencySymbol => _strategy.CurrencySymbol;
    }

}
