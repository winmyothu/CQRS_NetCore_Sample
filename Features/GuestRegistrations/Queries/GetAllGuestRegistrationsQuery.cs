using MediatR;
using System.Collections.Generic;
using CQRSExample.Models;

namespace CQRSExample.Features.GuestRegistrations.Queries
{
    public record GetAllGuestRegistrationsQuery() : IRequest<IEnumerable<GuestRegistration>>;
}
