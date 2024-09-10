using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DataAccess.Models
{
    public class Game
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name {  get; set; }

        [Required]
        public string Key { get; set; }

        public string? Description { get; set; }

        [InverseProperty(nameof(GameGenre.GameNavigation))]
        public GameGenre GameGenreNavigation { get; set; }

        [InverseProperty(nameof(GamePlatform.GameNavigation))]
        public GamePlatform GamePlatformNavigation { get; set; }
    }
}
