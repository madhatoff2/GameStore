using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.BusinessLogic.Models
{
    public class GamePlatform
    {
        public Guid GameId { get; set; }
        public Guid Platform { get; set; }

    }
}
