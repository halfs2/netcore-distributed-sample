using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NCD.Core.Mediator;
using System.Threading.Tasks;

namespace NCD.Customers.API.Controllers
{
    [Authorize]
    [Route("api/customer")]
    public class CustomerController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Closed()
        {
            return Ok("accesso liberado");
        }
    }    
}
