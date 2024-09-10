using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class Platform
    {
        public Guid? Id { get; set; }
        public string? Type { get; set; }
    }
}
