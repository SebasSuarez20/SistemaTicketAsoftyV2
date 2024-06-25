using SistemaTickets.Model;

namespace SistemaTickets.Interface.IModel
{
    public interface IticketsSupport
    {
        public Task<object> getAllMapAndSup();
        public Task<object> createTickets(createTicketModel model);
    }
}
