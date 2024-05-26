using SistemaTickets.Interface;
using SistemaTickets.Model;

namespace SistemaTickets.Services
{
    public class hubService : IHubconnection
    {

        private readonly IdbHandler<Users> _dbHandlerUser;
        private readonly IdbHandler<ticketssupport> _dbHandlerTickets;

        public hubService(IdbHandler<Users> dbHandlerUser, IdbHandler<ticketssupport> dbHandlerTickets) {
        
            _dbHandlerUser = dbHandlerUser;
            _dbHandlerTickets = dbHandlerTickets;
        }

        public async Task<Users> getInfoUser(int identity)
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

        public async Task updateTicket(int consecutive, int assigned)
        {
            try
            {
                var resultForTicket =await _dbHandlerTickets.GetAllAsyncForAll(s => s.Consecutive == consecutive);
                resultForTicket.First().AssignedTo = assigned;
                int resultIdControl = (int)resultForTicket.First().Idcontrol;
                await _dbHandlerTickets.UpdateAsyncAll(resultForTicket.First(), new ticketssupport { Idcontrol = resultIdControl });
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
