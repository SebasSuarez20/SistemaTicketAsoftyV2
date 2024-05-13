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

namespace SistemaTickets.Services
{
    public class HubConnection : Hub
    {

        public readonly IUser serviceUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HubConnection(IUser _serviceUser, IHttpContextAccessor httpContextAccessor)
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
            Users result = (Users) await serviceUser.getInfoUser(idHub);
            if (string.IsNullOrEmpty(result.HasConnection))
            {
            result.HasConnection = Context.ConnectionId.ToString();
            await serviceUser.updateUser(result);
            }
            await Clients.All.SendAsync("192.168.0.1", true);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Users result = (Users)await serviceUser.getInfoUser(int.Parse(_httpContextAccessor.HttpContext.User.FindFirst("miHub").Value));
            result.HasConnection = null;
            await serviceUser.updateUser(result);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
