using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DataAccess.Models
{
    
    public class Platform
    {
        [Key]
        public Guid Id { get; set; }

        [InverseProperty(nameof(GamePlatform.PlatformNavigation))]
        public List<GamePlatform> GamePlatformNavigation { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
