using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
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

        public virtual async Task<IEnumerable<Starship>> HandleAsync(IEnumerable<Starship> starships, CancellationToken ct)
        {
            if (_next is not null)
                return await _next.HandleAsync(starships, ct);

            return starships;
        }
    }

}
