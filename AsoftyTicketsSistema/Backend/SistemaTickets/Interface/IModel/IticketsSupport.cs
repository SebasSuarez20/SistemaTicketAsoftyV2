using SistemaTickets.Model;

namespace SistemaTickets.Interface.IModel
{
    public interface IticketsSupport
    {

        public Task<object> getAllTickets();

        public Task<object> createTickets(CreateTicketModel model);
        public Task<object> getAllMapAndSup();

    }
}
