using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.Models
{
    public class ParsedGenre
    {
        public Genre? genre { get; set; }
    }
}
