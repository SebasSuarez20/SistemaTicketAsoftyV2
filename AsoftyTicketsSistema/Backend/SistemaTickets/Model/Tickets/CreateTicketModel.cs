namespace SistemaTickets.Model
{
    public class CreateTicketModel
    {

        public string? header { get; set; }
        public List<IFormFile>? files { get; set; } 
    }
}
