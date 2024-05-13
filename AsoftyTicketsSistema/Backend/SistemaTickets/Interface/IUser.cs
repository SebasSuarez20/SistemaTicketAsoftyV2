using SistemaTickets.Model;

namespace SistemaTickets.Interface
{
    public interface IUser
    {
        public Task<object> getInfoUser(int identity);
        public Task updateUser(Users model);
    }
}
