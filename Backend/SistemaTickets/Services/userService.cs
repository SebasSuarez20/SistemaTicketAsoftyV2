using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;

namespace SistemaTickets.Services
{
    public class userService : IUser
    {
        private readonly IdbHandler<Users> _dbHandlerUser;
        public userService(IdbHandler<Users> dbHandlerUser) { 
         
             _dbHandlerUser = dbHandlerUser;
        }

        public async Task<int> updateThemeDefault(int themeColor)
        {
            try
            {
                int response = await _dbHandlerUser.UpdateForField("themeColor", themeColor);
                return response;
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
           
        }
    }
}
