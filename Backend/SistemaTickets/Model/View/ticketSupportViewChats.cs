using SistemaTickets.Model.Abstract;

namespace SistemaTickets.Model.View
{
    public class ticketSupportViewChats : dbUsername
    {

        public int? Consecutive { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? PhotoTickets { get; set; }
        public string? NameSupport { get; set; }
        public string? NameCompany { get; set; }
        public string? HasUnique { get; set; }
        public string? Message { get; set; }
        public DateTime? DateChat { get; set; }
        public bool? Enabled { get; set; }      

    }
}
