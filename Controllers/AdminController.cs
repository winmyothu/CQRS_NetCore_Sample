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
        public async Task<ActionResult<IEnumerable<GuestRegistration>>> GetAllRegistrations()
        {
            var query = new GetAllGuestRegistrationsQuery();
            var registrations = await _mediator.Send(query);
            return Ok(registrations);
        }

        [HttpGet("payments")]
        public async Task<ActionResult<IEnumerable<PaymentTransaction>>> GetAllPaymentTransactions()
        {
            var query = new GetAllPaymentTransactionsQuery();
            var payments = await _mediator.Send(query);
            return Ok(payments);
        }
    }
}
