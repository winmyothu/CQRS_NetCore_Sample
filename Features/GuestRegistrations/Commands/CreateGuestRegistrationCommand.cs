
using MediatR;
using Microsoft.AspNetCore.Http;
using System;

namespace CQRSExample.Features.GuestRegistrations.Commands
{
    /// <summary>
    /// Command to create a new guest registration.
    /// </summary>
    public record CreateGuestRegistrationCommand(
        string Name,
        DateTime DateOfBirth,
        string PassportNumber,
        string Nationality,
        string Nrc,
        string CurrentAddress,
        string PermanentAddress,
        IFormFileCollection AttachedFiles
    ) : IRequest<int>;
}
