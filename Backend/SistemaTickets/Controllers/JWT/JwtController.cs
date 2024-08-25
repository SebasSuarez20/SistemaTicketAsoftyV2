using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SistemaTickets.Controllers.JWT
{
    [Route("api/RIzFe3ERr+dEyYxpHLkkcZj8VLOPIh5IGPE+Un6tEOM=")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class JwtController : ControllerBase
    {

        private IConfiguration _config;

        public JwtController(IConfiguration config) { 
           _config = config;
           httpUtils.key = _config.GetSection("JWT:Key").Value;
        }


        [HttpGet("qBxaIJFATtc5xC/+k/J4H2/joKbisL063cPCRs9dEqc=")]
        public IActionResult createJwt()
        {
            return Ok(httpUtils.response(httpUtils.generateToken(), "Se creo correctamente"));
        }

    }
}
