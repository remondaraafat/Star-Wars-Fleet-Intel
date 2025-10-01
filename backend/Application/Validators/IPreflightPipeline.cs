using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    internal interface IPreflightPipeline
    {
        bool BeValidNumberOrUnknown(string? value);
    }
}
