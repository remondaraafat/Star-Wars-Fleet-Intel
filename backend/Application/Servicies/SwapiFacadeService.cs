//using Application.Interfaces;
//using Domain.Models;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using Infrastructure.Swapi;
//namespace Application.Servicies
//{


//    public class SwapiFacadeService : ISwapiFacadeService
//    {
//       // private readonly SwapiClient _client;
//        private readonly IPreflightPipeline _preflight;
//        private readonly IStarshipEnricher _enricher;

//        public SwapiFacade(SwapiClient client, IPreflightPipeline preflight, IStarshipEnricher enricher)
//        {
//            _client = client;
//            _preflight = preflight;
//            _enricher = enricher;
//        }

//        public async Task<Starship> GetEnrichedStarshipByIdAsync(int id, CancellationToken ct)
//        {
//            var raw = await _client.GetStarshipByIdAsync(id, ct);
//            var domain = raw.ToDomain(); // mapping
//            await _preflight.ExecuteAsync(domain, ct); // chain of responsibility
//            await _enricher.EnrichAsync(domain, ct);   // additional enrichment (e.g., pilot names, films)
//            return domain;
//        }
//    }
//}
