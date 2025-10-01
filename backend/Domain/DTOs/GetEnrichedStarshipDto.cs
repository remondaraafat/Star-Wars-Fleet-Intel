using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class GetEnrichedStarshipDto : BaseEntity
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string CostInCredits { get; set; }
        public string Length { get; set; }
        public string Crew { get; set; }
        public string Passengers { get; set; }
        public string MaxAtmospheringSpeed { get; set; }
        public string HyperdriveRating { get; set; }
        public string MGLT { get; set; }
        public string CargoCapacity { get; set; }
        public string Consumables { get; set; }
        public List<GetFilmDto> Films { get; set; }
        public List<GetPersonDto> Pilots { get; set; }
        //currency conversion outputs
        public string CurrencySymbol { get; set; }
        public decimal ConvertedCost { get; set; }
        // Decorator outputs
        public int ShieldBoost { get; set; } = 0;
        public int TargetingAccuracy { get; set; } = 0;
    }
}
