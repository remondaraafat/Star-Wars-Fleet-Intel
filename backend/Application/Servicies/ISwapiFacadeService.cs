using Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Servicies
{
    public interface ISwapiFacadeService
    {
        Task<IEnumerable<GetStarshipsDto>> GetStarshipsAsync(string? search = null, CancellationToken ct = default);
        Task<GetEnrichedStarshipDto> GetEnrichedStarshipByIdAsync(int id, CancellationToken ct = default);
    }
}
