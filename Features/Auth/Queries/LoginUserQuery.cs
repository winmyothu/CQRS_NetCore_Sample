using MediatR;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Queries
{
    public record LoginUserQuery(string Username, string Password) : IRequest<AuthResult>;
}
