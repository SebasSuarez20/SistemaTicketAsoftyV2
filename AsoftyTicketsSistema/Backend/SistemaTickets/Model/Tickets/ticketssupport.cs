using Newtonsoft.Json.Converters;
using SistemaTickets.Model.Abstract;
using System.ComponentModel.DataAnnotations;

namespace SistemaTickets.Model
{
    public class ticketssupport :DbUsername
    {

        [Key]
        public int? Idcontrol { get; set; }
        public int Consecutive { get; set; }
        public string Title { get; set; }
        public string Aerea { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public string? Priority { get; set; }
        public string? PhotoDescription { get; set; }
        public int? AssignedTo { get; set; }
        public DateTime? Date_Update { get; set; }
        public Boolean? Enabled { get; set; }
    }

    public enum Status
    {
        Open,
        InProgress,
        Result,
        Close
    }
}
