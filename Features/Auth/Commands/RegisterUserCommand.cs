using MediatR;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Commands
{
    public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<AuthResult>;
}
