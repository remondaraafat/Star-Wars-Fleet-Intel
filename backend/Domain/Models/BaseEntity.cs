using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common;

namespace StarWars.Models
{
    public class BaseEntity:IBaseModel
    {
        public int Id { get; set; }


    }
}
