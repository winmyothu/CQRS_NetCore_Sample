using MediatR;
using CQRSExample.Data;
using CQRSExample.Models;
using CQRSExample.Features.GuestRegistrations.Queries;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CQRSExample.Features.GuestRegistrations.Handlers
{
    /// <summary>
    /// Handles the retrieval of a guest registration by its ID.
    /// </summary>
    public class GetGuestRegistrationByIdHandler : IRequestHandler<GetGuestRegistrationByIdQuery, GuestRegistration>
    {
        private readonly AppDbContext _context;

        public GetGuestRegistrationByIdHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GuestRegistration> Handle(GetGuestRegistrationByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.GuestRegistrations.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        }
    }
}
