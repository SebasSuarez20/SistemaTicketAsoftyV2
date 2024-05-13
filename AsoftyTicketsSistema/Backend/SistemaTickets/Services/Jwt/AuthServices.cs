using SistemaTickets.Interface.IJwt;
using System.Security.Claims;

namespace SistemaTickets.Services.Jwt
{
    public class AuthServices : IJwt
    {

        private readonly IHttpContextAccessor _contextAccessor;
        public string valor;

        public AuthServices(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserName()
        {
            var getUser = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name);
            return getUser?.Value;
        }

        public int GetRoleUser()
        {
            var getRol = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role);
            return int.Parse(getRol.Value);
        }

        public string GetIdHub()
        {
            var _context = _contextAccessor.HttpContext;
            var idHub = _context.User.FindFirst(ClaimTypes.Authentication);
            return idHub.Value.ToString();
        }
    }
}
