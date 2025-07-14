using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.GuestRegistrations.Queries
{
    /// <summary>
    /// Query to get a guest registration by its ID.
    /// </summary>
    public record GetGuestRegistrationByIdQuery(int Id) : IRequest<GuestRegistration>;
}
