﻿using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class PersonDto:BaseEntity
    {

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("height")]
        public string Height { get; set; } = string.Empty;

        [JsonPropertyName("mass")]
        public string Mass { get; set; } = string.Empty;

        [JsonPropertyName("hair_color")]
        public string HairColor { get; set; } = string.Empty;

        [JsonPropertyName("skin_color")]
        public string SkinColor { get; set; } = string.Empty;

        [JsonPropertyName("eye_color")]
        public string EyeColor { get; set; } = string.Empty;

        [JsonPropertyName("birth_year")]
        public string BirthYear { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;

        [JsonPropertyName("homeworld")]
        public string Homeworld { get; set; } = string.Empty; 

       
        [JsonPropertyName("created")]
        public string Created { get; set; } = string.Empty;

        [JsonPropertyName("edited")]
        public string Edited { get; set; } = string.Empty;

        
    }
}
