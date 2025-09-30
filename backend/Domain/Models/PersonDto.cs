using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PersonDto:BaseEntity
    {

        public string Name { get; set; }
        public string Height { get; set; }
        public string Mass { get; set; }
        public string HairColor { get; set; }
        public string SkinColor { get; set; }
        public string EyeColor { get; set; }
        public string BirthYear { get; set; }
        public string Gender { get; set; }
        public string Homeworld { get; set; } // URL
        public IEnumerable<string> Films { get; set; }    // URLs
        public IEnumerable<string> Species { get; set; }  // URLs
        public IEnumerable<string> Vehicles { get; set; } // URLs
        public IEnumerable<string> Starships { get; set; } // URLs
        public string Created { get; set; }
        public string Edited { get; set; }
    }
}
