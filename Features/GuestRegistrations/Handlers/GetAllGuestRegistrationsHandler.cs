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
    public class GetAllGuestRegistrationsHandler : IRequestHandler<GetAllGuestRegistrationsQuery, IEnumerable<GuestRegistration>>
    {
        private readonly AppDbContext _context;

        public GetAllGuestRegistrationsHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GuestRegistration>> Handle(GetAllGuestRegistrationsQuery request, CancellationToken cancellationToken)
        {
            return await _context.GuestRegistrations.ToListAsync(cancellationToken);
        }
    }
}
