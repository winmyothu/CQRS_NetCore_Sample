using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Models;
using Microsoft.EntityFrameworkCore;
using CQRSExample.Features.GuestRegistrations.Queries;

namespace CQRSExample.Features.GuestRegistrations.Handlers
{
    public class GetAllGuestRegistrationsHandler : IRequestHandler<GetAllGuestRegistrationsQuery, PaginatedResult<GuestRegistration>>
    {
        private readonly AppDbContext _context;

        public GetAllGuestRegistrationsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedResult<GuestRegistration>> Handle(GetAllGuestRegistrationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.GuestRegistrations.AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginatedResult<GuestRegistration>(items, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
