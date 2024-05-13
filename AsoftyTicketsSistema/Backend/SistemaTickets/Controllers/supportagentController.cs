using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;

namespace SistemaTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class supportagentController : Controller
    {

        private readonly ISupportagent _service;

        public supportagentController(ISupportagent service)
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
