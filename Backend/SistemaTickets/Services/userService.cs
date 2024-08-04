using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;

namespace SistemaTickets.Services
{
    public class userService : IUser
    {
        private readonly IdbHandler<Users> _dbHandlerUser;
        private readonly IdbHandler<informationUser> _dbHandlerInformationUser;
        public userService(IdbHandler<Users> dbHandlerUser, IdbHandler<informationUser> dbHandlerInformationUser) { 
         
             _dbHandlerUser = dbHandlerUser;
            _dbHandlerInformationUser = dbHandlerInformationUser;
        }

        public async Task<dynamic> createUser(createUserModel user)
        {


            try
            {
                await _dbHandlerUser.CreateAllAsync(user.header);
                await _dbHandlerInformationUser.CreateAllAsync(user.body);
            }
            catch(Exception ex) 
            {
                return new
                {
                    status = StatusCodes.Status400BadRequest,
                    message = ex.Message,
                };
            }
            return new 
            { 
                status = StatusCodes.Status200OK,
                message = "Se creo Correctamente el usuario",
            };
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
