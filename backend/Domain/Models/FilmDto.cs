using StarWars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FilmDto:BaseEntity
    {

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("episode_id")]
        public int EpisodeId { get; set; }

        [JsonPropertyName("opening_crawl")]
        public string OpeningCrawl { get; set; } = string.Empty;

        [JsonPropertyName("director")]
        public string Director { get; set; } = string.Empty;

        [JsonPropertyName("producer")]
        public string Producer { get; set; } = string.Empty;

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } = string.Empty;

       

       

        [JsonPropertyName("created")]
        public string Created { get; set; } = string.Empty;

        [JsonPropertyName("edited")]
        public string Edited { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }
}
