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
        Task<IEnumerable<Starship>> GetStarshipsAsync(string endpoint, CancellationToken ct);
        Task<Starship> GetStarshipByIdAsync(int id, CancellationToken ct);
        Task<Person> GetPersonByIdAsync(int id, CancellationToken ct);
        Task<Film> GetFilmByIdAsync(int id, CancellationToken ct);
    }
}
