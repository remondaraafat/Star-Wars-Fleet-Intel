using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SwapiProvider
{
    public interface IFakeSwapiProvider
    {
        Task<IEnumerable<Starship>> GenerateFakeStarshipsAsync(string? search, CancellationToken ct = default);
        Task<Starship> GenerateFakeStarshipAsync(int id, CancellationToken ct = default);
        Task<Person> GenerateFakePersonAsync(int id, CancellationToken ct = default);
        Task<Film> GenerateFakeFilmAsync(int id, CancellationToken ct = default);
    }
}
