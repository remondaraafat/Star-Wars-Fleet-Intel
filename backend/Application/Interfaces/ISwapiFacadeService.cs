using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISwapiFacadeService
    {
        Task<IEnumerable<Starship>> GetStarshipsAsync(string? search = null, CancellationToken ct = default);
    }
}
