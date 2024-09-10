using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.BusinessLogic.Models
{
    public class GameGenre
    {
        public Guid GameId { get; set; }
        public Guid GenreId { get; set; }
    }
}
