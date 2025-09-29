using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{

    public interface IStarshipListHandler
    {
        Task<IEnumerable<Starship>> HandleAsync(IEnumerable<Starship> starships, CancellationToken ct = default);
    }
}
