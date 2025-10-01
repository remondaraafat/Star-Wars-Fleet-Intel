using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChainHandler
{
    public class SanitizationHandler : StarshipHandlerBase
    {
        public override Task<IEnumerable<StarshipRequestDto>> HandleAsync(IEnumerable<StarshipRequestDto> starships, CancellationToken ct)
        {
            foreach (var starship in starships)
            {
                starship.Name = starship.Name?.Trim();
                starship.Model = starship.Model?.Trim();
            }

            return base.HandleAsync(starships, ct);
        }
    }


}
