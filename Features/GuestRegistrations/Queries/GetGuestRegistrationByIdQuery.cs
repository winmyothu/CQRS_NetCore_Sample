using MediatR;
using CQRSExample.Models;

namespace CQRSExample.Features.GuestRegistrations.Queries
{
    public class GetGuestRegistrationByIdQuery : IRequest<GuestRegistration>
    {
        public int Id { get; set; }

        public GetGuestRegistrationByIdQuery(int id)
        {
            Id = id;
        }
    }
}
