using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model.View
{
    public class TicketMapAndSupView : DbUsername
    {
        [Key]
        public int? Idcontrol { get; set; }
        public int? Consecutive { get; set; }
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Status { get; set; }
        public int? AssignedTo { get; set; }
        public string? HasUnique { get; set; }
        public bool? Enabled { get; set; }
    }
}
