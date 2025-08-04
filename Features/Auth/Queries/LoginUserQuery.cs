using MediatR;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Queries
{
    public record LoginUserQuery(string Email, string Password) : IRequest<AuthResult>;
}
