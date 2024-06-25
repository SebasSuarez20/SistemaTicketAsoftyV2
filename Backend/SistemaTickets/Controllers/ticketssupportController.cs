using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SistemaTickets.Interface.IModel;
using SistemaTickets.Model;
using SistemaTickets.Model.View;
using static System.Net.Mime.MediaTypeNames;

namespace SistemaTickets.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ticketssupportController : Controller
    {

        private readonly IticketsSupport _service;

        public ticketssupportController(IticketsSupport service)
        {
            this._service = service;
        }


        [HttpPost("CreateTickets")]
        public async Task<object> CreateTickets([FromForm] createTicketModel model)
        {
            return Ok(await _service.createTickets(model));
        }

        [HttpGet("GetAllMapAndSup")]
        public async Task<object> GetAllMapAndSup()
        {
            return Ok(await _service.getAllMapAndSup());
        }


    }
}
