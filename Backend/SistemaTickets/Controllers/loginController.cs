using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface.IModel;

namespace SistemaTickets.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class loginController : Controller
    {

        private readonly ILogin _service;

        public loginController(ILogin service)
        {
            _service = service;
        }

        [HttpGet("authService")]
        public Task<object> authService(string user, string pswd)
        {
            return _service.authLoginSupport(user, pswd);
        }

    }
}
