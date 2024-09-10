using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DataAccess.Models
{
    public class Genre
    {
        [Key]
        public Guid Id { get; set; }

        [InverseProperty(nameof(GameGenre.GenreNavigation))]
        public List<GameGenre> GameGenreNavigation { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid? ParentGenreId { get; set; }
    }
}
