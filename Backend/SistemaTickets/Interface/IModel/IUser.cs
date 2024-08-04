using SistemaTickets.Model;

namespace SistemaTickets.Interface.IModel
{
    public interface IUser
    {
        public Task<dynamic> createUser(createUserModel user);
        public Task<int> updateThemeDefault(int themeColor);
    }
}
