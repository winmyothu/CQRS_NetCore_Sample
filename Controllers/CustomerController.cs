using CQRSExample.Features.Customers.Commands;
using CQRSExample.Features.Customers.Queries;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CQRSExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var customers = await _mediator.Send(query);
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var query = new GetCustomerByIdQuery(id);
            var customer = await _mediator.Send(query);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
        {
            try
            {
                var customerId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customerId }, customerId);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            try
            {
                var result = await _mediator.Send(command);
                if (!result)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var command = new DeleteCustomerCommand(id);
            var result = await _mediator.Send(command);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
