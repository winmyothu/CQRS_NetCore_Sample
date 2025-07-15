using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Models;
using CQRSExample.Features.Auth.Commands;
using CQRSExample.Features.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace CQRSExample.Features.Auth.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResult>
    {
        private readonly AppDbContext _context;

        public RegisterUserHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(u => u.Username == request.Username, cancellationToken))
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Username already exists." } };
            }

            // In a real application, use a strong password hashing library like BCrypt.Net-Core
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password); // Placeholder for actual hashing

            var isAdmin = !await _context.Users.AnyAsync(cancellationToken);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = isAdmin ? "Admin" : "User"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthResult { Succeeded = true };
        }
    }
}
