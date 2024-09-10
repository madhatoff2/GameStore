using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.BusinessLogic.Models
{
    public class FullGame
    {
        public Game game { get; set; } = new Game();
        public List<Guid> genres { get; set; } = new List<Guid>();
        public List<Guid> platforms { get; set; } = new List<Guid>();
    }

}
