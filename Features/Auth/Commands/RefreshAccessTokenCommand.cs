using MediatR;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Commands
{
    public record RefreshAccessTokenCommand(string RefreshToken, string IpAddress) : IRequest<AuthResult>;
}
