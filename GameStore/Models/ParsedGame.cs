using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class ParsedGame
    {
        public Game? game { get; set; }
        public List<Guid>? genres { get; set; }
        public List<Guid>? platforms { get; set; }
    }

}
