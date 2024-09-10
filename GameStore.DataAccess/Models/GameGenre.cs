using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DataAccess.Models
{
    public class GameGenre
    {
        
        public Guid GameId { get; set; }

        [ForeignKey("GameId")]
        public Game GameNavigation { get; set; }

        
        public Guid GenreId { get; set; }

        [ForeignKey("GenreId")]
        public Genre GenreNavigation { get; set; }
    }
}
