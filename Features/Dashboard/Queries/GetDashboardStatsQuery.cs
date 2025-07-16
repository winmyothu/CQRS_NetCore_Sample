using MediatR;
using CQRSExample.Features.Dashboard.Models;

namespace CQRSExample.Features.Dashboard.Queries
{
    public class GetDashboardStatsQuery : IRequest<DashboardStats>
    {
    }
}
