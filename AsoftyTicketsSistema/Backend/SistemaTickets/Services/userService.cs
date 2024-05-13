using SistemaTickets.Interface;
using SistemaTickets.Interface.IJwt;
using SistemaTickets.Model;
using System.Security.Claims;
using System.Security.Principal;

namespace SistemaTickets.Services
{
    public class userService : IUser
    {

        private readonly IdbHandler<Users> _dbHandlerUser;
        private IHttpContextAccessor _contextAccessor;

        public userService(IdbHandler<Users> dbHandlerUser, IHttpContextAccessor contextAccessor) {
        
             this._dbHandlerUser = dbHandlerUser;
            _contextAccessor = contextAccessor;
        }

        public async Task<object> getInfoUser(int identity)
        {

            var result = await _dbHandlerUser.GetAllAsyncForAll(s=>s.Idcontrol == identity);
            return result.First() ?? null;
        }

        public async Task updateUser(Users model)
        {
            try
            {
                await _dbHandlerUser.UpdateAsyncAll(model, new Users { Idcontrol = model.Idcontrol });
            }catch(Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
            
        }
    }
}
