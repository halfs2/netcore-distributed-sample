using Microsoft.AspNetCore.Mvc;
using NCD.Core.Mediator;
using NCD.Customers.API.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCD.Customers.API.Controllers
{
    [Route("api/customer")]
    public class CustomerController : Controller
    {
        private readonly IMediatorHandler _mediatorHandler;

        public CustomerController(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerRegisterDTO dto)
        {
            var result = await _mediatorHandler.SendCommand(new CustomerRegisterCommand(dto.Id, dto.Name, dto.Email, dto.Document));

            if (result.IsValid)
                return Created("Customer", dto);

            return BadRequest(result.Errors);
        }
    }

    public class CustomerRegisterDTO
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }
}
