using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Strategies
{

    public interface ICostConversionStrategy
    {
        decimal? ConvertCost(string costInCredits, string targetCurrency);
    }
}
