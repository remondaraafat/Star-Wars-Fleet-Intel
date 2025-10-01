using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ChainHandler
{

    public interface IStarshipHandler
    {
        Task<IEnumerable<StarshipRequestDto>> HandleAsync(IEnumerable<StarshipRequestDto> starships, CancellationToken ct = default);
        IStarshipHandler SetNext(IStarshipHandler next);
    }
}
