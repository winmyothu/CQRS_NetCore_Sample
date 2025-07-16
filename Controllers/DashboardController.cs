using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using CQRSExample.Features.Dashboard.Queries;
using Microsoft.AspNetCore.Authorization;

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
    }
}
