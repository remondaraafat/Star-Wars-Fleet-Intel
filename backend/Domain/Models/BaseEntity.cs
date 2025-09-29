using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace StarWars.Models
{
    public class BaseEntity:IBaseModel
    {
        public int Id
        {
            get => int.TryParse(Url?.Split('/').LastOrDefault(s => !string.IsNullOrEmpty(s)), out var id) ? id : 0;//derive Id from Url in get only
            set { }
        }
        public string Url { get; set; }


    }
}
