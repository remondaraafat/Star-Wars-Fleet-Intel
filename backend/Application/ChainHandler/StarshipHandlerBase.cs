using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTOs;
using FluentValidation;
using FluentValidation.Results;
namespace Application.ChainHandler
{

    public abstract class StarshipHandlerBase : IStarshipHandler
    {
        private IStarshipHandler? _next;

        public IStarshipHandler SetNext(IStarshipHandler next)
        {
            _next = next;
            return next;
        }

        public virtual async Task<IEnumerable<StarshipRequestDto>> HandleAsync(IEnumerable<StarshipRequestDto> starships, CancellationToken ct)
        {
            if (_next is not null)
                return await _next.HandleAsync(starships, ct);

            return starships;
        }
    }

}
