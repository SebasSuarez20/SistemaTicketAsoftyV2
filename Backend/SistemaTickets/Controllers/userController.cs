using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;

namespace SistemaTickets.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class userController : Controller
    {

        private readonly IUser _service;

        public userController(IUser service)
        {
            _service = service;
        }

        [HttpPost("createUser")]

        public async Task<IActionResult> createUser([FromBody] createUserModel user)
        {
            return Ok(await _service.createUser(user));
        }

        [HttpGet("updateThemeDefault")]
        public async Task<IActionResult> updateThemeDefault(int themeColor)
        {
            return Ok(await _service.updateThemeDefault(themeColor));
        }


    }
}
