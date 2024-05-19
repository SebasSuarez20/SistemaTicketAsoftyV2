using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MySqlX.XDevAPI.Common;
using SistemaTickets.Interface;
using SistemaTickets.Interface.IJwt;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SistemaTickets.Services.SignalR
{
    public class HubConnection : Hub
    {

        public readonly IHubconnection serviceUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HubConnection(IHubconnection _serviceUser, IHttpContextAccessor httpContextAccessor)
        {
            serviceUser = _serviceUser;
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("192.168.0.1", true);
            await Clients.All.SendAsync("192.168.0.2", true);
            await base.OnConnectedAsync();
        }

        public async Task UpdateLogged(int idHub)
        {
            var identity = Context.User.Identity as ClaimsIdentity;
            var claim = new Claim("miHub", idHub.ToString());
            identity.AddClaim(claim);
            Users result = await serviceUser.getInfoUser(idHub);
            if (string.IsNullOrEmpty(result.HasConnection))
            {
                result.HasConnection = Context.ConnectionId.ToString();
                await serviceUser.updateUser(result);
            }
            await Clients.All.SendAsync("192.168.0.1", true);
        }

        public async Task SendMessageToClient(int assigned, int username, int consecutive)
        {

            string[] message = { "Se te ha asignado un nuevo ticket. Por favor, verifica.", $"El Ticket #{consecutive} ha sido asignado.Ahora puede iniciar el chat"};

            Users connectLogged = await serviceUser.getInfoUser(int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("miHub").Value));
            Users connectOfAssigned = await serviceUser.getInfoUser(assigned);
            Users connectOfCompany =await  serviceUser.getInfoUser(username);

            await serviceUser.updateTicket(consecutive, assigned);

            try
            {
                if (connectOfAssigned.HasConnection == null && connectOfCompany.HasConnection == null)
                {
                    await Clients.Clients(connectLogged.HasConnection).SendAsync("192.168.0.2",
                        "Ninguno se encuentra conectado en estos momentos pero ya los cambios estan hechos.");
                }
                else
                {
                    await Clients.Clients(connectOfAssigned.HasConnection ?? connectLogged.HasConnection).
                        SendAsync("192.168.0.2", !string.IsNullOrEmpty(connectOfAssigned.HasConnection) ? message[0] : "El usuario aún no está conectado, pero podrá ver los cambios una vez lo haga.");
                    await Clients.Clients(connectOfCompany.HasConnection ?? connectLogged.HasConnection).
                        SendAsync("192.168.0.2", !string.IsNullOrEmpty(connectOfCompany.HasConnection) ? message[1] : "El usuario aún no está conectado, pero podrá ver los cambios una vez lo haga.");
                }
            }catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Users result = await serviceUser.getInfoUser(int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst("miHub")?.Value));
            result.HasConnection = null;
            await serviceUser.updateUser(result);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
