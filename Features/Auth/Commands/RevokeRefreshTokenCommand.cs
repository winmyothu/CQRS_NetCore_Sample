using MediatR;

namespace CQRSExample.Features.Auth.Commands
{
    public record RevokeRefreshTokenCommand(string RefreshToken, string IpAddress) : IRequest<bool>;
}
