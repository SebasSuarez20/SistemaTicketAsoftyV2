using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaTickets.Model.Abstract
{
    public abstract class dbUsername
    {

        [Key]
        public int? Idcontrol { get; set; } = null;
        [Column]
        public int? Username { get; set; } = null;
        
    }
}
