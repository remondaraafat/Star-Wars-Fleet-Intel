using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FilmDto:BaseEntity
    {

        public string Title { get; set; }
        public int EpisodeId { get; set; }
        public string OpeningCrawl { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string ReleaseDate { get; set; }
        public IEnumerable<string> Characters { get; set; } // URLs
        public IEnumerable<string> Planets { get; set; }   // URLs
        public IEnumerable<string> Starships { get; set; } // URLs
        public IEnumerable<string> Vehicles { get; set; }  // URLs
        public IEnumerable<string> Species { get; set; }   // URLs
        public string Created { get; set; }
        public string Edited { get; set; }
    }
}
