using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CQRSExample.Features.GuestRegistrations.Commands;
using CQRSExample.Features.GuestRegistrations.Queries;
using CQRSExample.Models;

namespace CQRSExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GuestRegistrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuestRegistrationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateGuestRegistration([FromForm] CreateGuestRegistrationCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuestRegistrationById(int id)
        {
            var query = new GetGuestRegistrationByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GuestRegistration>>> GetAllRegistrations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllGuestRegistrationsQuery(pageNumber, pageSize, null, null, null);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyRegistrations()
        {
            var query = new GetMonthlyRegistrationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
