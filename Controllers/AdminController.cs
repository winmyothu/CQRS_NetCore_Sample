using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using CQRSExample.Models;
using CQRSExample.Features.GuestRegistrations.Queries;
using CQRSExample.Features.Payments.Queries;

namespace CQRSExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Only accessible by users with 'Admin' role
    public class AdminController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("registrations")]
        public async Task<ActionResult<PaginatedResult<GuestRegistration>>> GetAllRegistrations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllGuestRegistrationsQuery(pageNumber, pageSize);
            var registrations = await _mediator.Send(query);
            return Ok(registrations);
        }

        [HttpGet("payments")]
        public async Task<ActionResult<PaginatedResult<PaymentTransaction>>> GetAllPaymentTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllPaymentTransactionsQuery(pageNumber, pageSize);
            var payments = await _mediator.Send(query);
            return Ok(payments);
        }
    }
}
