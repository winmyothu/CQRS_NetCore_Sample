using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Features.Dashboard.Models;
using CQRSExample.Features.Dashboard.Queries;
using Microsoft.EntityFrameworkCore;

namespace CQRSExample.Features.Dashboard.Handlers
{
    public class GetDashboardStatsHandler : IRequestHandler<GetDashboardStatsQuery, DashboardStats>
    {
        private readonly AppDbContext _context;

        public GetDashboardStatsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardStats> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var stats = new DashboardStats
            {
                TotalGuests = await _context.GuestRegistrations.CountAsync(cancellationToken),
                TotalTransactions = await _context.PaymentTransactions.CountAsync(cancellationToken),
                TotalRegistrations = await _context.Users.CountAsync(cancellationToken)
            };

            return stats;
        }
    }
}
