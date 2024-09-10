using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class GameGenre
    {
        public Guid? GameId { get; set; }
        public Guid? GenreId { get; set; }
    }
}
