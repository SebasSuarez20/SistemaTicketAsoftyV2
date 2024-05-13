using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model.View
{
    public class consecticketview
    {

        [Key]
        public int? id { get; set; }
        public int? consecutive { get; set; }

        public bool? enabled { get; set; }  

    }
}
