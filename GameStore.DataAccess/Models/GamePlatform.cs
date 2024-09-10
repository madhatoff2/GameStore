using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DataAccess.Models
{
    public class GamePlatform
    {
        [Key]
        public Guid GameId { get; set; }

        [ForeignKey(nameof(GameId))]
        public Game GameNavigation { get; set; }

        [Required]
        public Guid Platform { get; set; }

        [ForeignKey(nameof(Platform))]
        public Platform? PlatformNavigation { get; set; }
    }
}
