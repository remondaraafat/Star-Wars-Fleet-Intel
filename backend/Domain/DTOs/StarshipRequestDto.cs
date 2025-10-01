using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class StarshipRequestDto:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Cost_In_Credits { get; set; } = string.Empty;
        public string Length { get; set; } = string.Empty;
        public string Max_Atmosphering_Speed { get; set; } = string.Empty;
        public string Crew { get; set; } = string.Empty;
        public string Passengers { get; set; } = string.Empty;
        public string Cargo_Capacity { get; set; } = string.Empty;
        public string Consumables { get; set; } = string.Empty;
        public string Hyperdrive_Rating { get; set; } = string.Empty;
        public string MGLT { get; set; } = string.Empty;
        public string Starship_Class { get; set; } = string.Empty;
        public List<string> Pilots { get; set; } = new();
        public List<string> Films { get; set; } = new();
        public DateTime Created { get; set; }
        public DateTime Edited { get; set; }
    }
}
