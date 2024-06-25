using SistemaTickets.Model;

namespace SistemaTickets.Interface
{
    public interface IHubconnection
    {
        public Task<Users> getInfoUser(int identity);
        public Task updateUser(Users model);
        public Task updateTicket(int consecutive, int assigned);
    }
}
