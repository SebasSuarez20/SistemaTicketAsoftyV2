using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaTickets.Model.View
{
    public class TicketMapAndSupView : dbUsername
    {
        public int? Consecutive { get; set; }
        public string? Area { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public int? AssignedTo { get; set; }
        public string? HasUnique { get; set; }
        [Column]
        public bool? Enabled { get; set; }
    }
}
