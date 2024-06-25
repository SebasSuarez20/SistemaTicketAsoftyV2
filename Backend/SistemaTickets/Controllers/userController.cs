using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface;
using SistemaTickets.Interface.IModel;

namespace SistemaTickets.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class userController : Controller
    {

        private readonly ILogin _service;

        public userController(ILogin service)
        {
            _service = service;
        }


        [HttpGet("authService")]
        public Task<object> authService(string user, string pswd)
        {
            return _service.authLoginSupport(user, pswd);
        }

        [HttpGet("SpExample")]
        public IActionResult spExample(string user, string pswd)
        {
            return Ok(_service.authLoginSupport(user,pswd));
        }

    }
}
