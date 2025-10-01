using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{

    public interface ICurrencyConversionStrategy
    {
        decimal Convert(decimal galacticCredits);
        string CurrencySymbol { get; }
    }
}
