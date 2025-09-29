using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common
{
    public interface IBaseModel
    {
        public int Id { get; set; }

    }
}
