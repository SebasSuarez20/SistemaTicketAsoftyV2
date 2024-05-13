using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;

namespace SistemaTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
