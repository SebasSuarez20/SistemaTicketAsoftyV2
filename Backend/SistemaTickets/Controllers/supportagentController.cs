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



    }
}
