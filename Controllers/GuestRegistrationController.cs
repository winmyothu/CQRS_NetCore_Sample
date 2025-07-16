using MediatR;
using Microsoft.AspNetCore.Mvc;
using CQRSExample.Features.GuestRegistrations.Commands;
using CQRSExample.Features.GuestRegistrations.Queries;
using System.Threading.Tasks;

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

        /// <summary>
        /// Creates a new guest registration.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateGuestRegistrationCommand command)
        {
            var guestRegistrationId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = guestRegistrationId }, null);
        }

        /// <summary>
        /// Gets a guest registration by its ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetGuestRegistrationByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Gets all guest registrations with pagination.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllGuestRegistrationsQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
