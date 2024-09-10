using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.BusinessLogic.Models
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? ParentGenreId { get; set; }
    }
}
