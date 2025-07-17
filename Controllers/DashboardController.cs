using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using CQRSExample.Features.Dashboard.Queries;
using Microsoft.AspNetCore.Authorization;
using CQRSExample.Features.GuestRegistrations.Queries;
using CQRSExample.Features.Payments.Queries;

namespace CQRSExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardStats()
        {
            var query = new GetDashboardStatsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("monthly-registrations")]
        public async Task<IActionResult> GetMonthlyRegistrations()
        {
            var query = new GetMonthlyRegistrationsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("yearly-registration-fees")]
        public async Task<IActionResult> GetYearlyRegistrationFee()
        {
            var query = new GetYearlyRegistrationFeesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }


    }
}
