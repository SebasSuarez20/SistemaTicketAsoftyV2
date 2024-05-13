using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface.IModel;

namespace SistemaTickets.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class fileController : Controller
    {

        private readonly IFile _service;

        public fileController(IFile service) {
        
             _service = service;
        }


        [HttpPost("CreateFile")]
        public async Task<object> CreateFile(IFormFile file)
        {
            try
            {
                return Ok(await _service.createFile(file));
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
