using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISwapiClient
    {
        Task<IEnumerable<StarshipRequestDto>> GetStarshipsAsync(string endpoint, CancellationToken ct);
        Task<StarshipRequestDto> GetStarshipByIdAsync(int id, CancellationToken ct);
        Task<Person> GetPersonByIdAsync(int id, CancellationToken ct);
        Task<Film> GetFilmByIdAsync(int id, CancellationToken ct);
    }
}
