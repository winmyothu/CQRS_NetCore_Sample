using MediatR;
using System.Collections.Generic;
using CQRSExample.Models;

namespace CQRSExample.Features.GuestRegistrations.Queries
{
    public record GetAllGuestRegistrationsQuery(int PageNumber, int PageSize) : IRequest<PaginatedResult<GuestRegistration>>;
}
