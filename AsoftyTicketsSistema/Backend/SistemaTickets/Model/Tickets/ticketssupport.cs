using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model
{
    public class ticketssupport :DbUsername
    {

        [Key]
        public int? Idcontrol { get; set; }
        public int Consecutive { get; set; }
        public string? Title { get; set; }
        public string Aerea { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public string? PhotoDescription { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? Date_Update { get; set; } = DateTime.Now;
        public bool? Enabled { get; set; }
    }
}
