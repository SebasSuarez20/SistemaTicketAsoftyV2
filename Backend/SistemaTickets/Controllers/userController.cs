using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface;
using SistemaTickets.Interface.IModel;

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

        [HttpGet("updateThemeDefault")]
        public async Task<IActionResult> updateThemeDefault(int themeColor)
        {
            return Ok(await _service.updateThemeDefault(themeColor));
        }
    }
}
