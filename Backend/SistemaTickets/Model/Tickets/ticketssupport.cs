using Newtonsoft.Json.Converters;
using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model
{
    public class ticketssupport : dbUsername
    {
        public int? Consecutive { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Area { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int SoftwareApplication { get; set; }
        public string? CodeConnectionSoftware { get; set; }
        public string? PhotoDescription { get; set; }
        public int? AssignedTo { get; set; }
        public bool? Enabled { get; set; } 
    }

    public enum Status
    {
        Open,
        InProgress,
        Result,
        Close
    }
}
