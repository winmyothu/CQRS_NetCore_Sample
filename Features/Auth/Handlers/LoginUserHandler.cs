using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Features.Auth.Queries;
using CQRSExample.Features.Auth.Models;
using CQRSExample.Features.Auth.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CQRSExample.Models; // Ensure this is present
using System.Linq;

namespace CQRSExample.Features.Auth.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserQuery, AuthResult>
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public LoginUserHandler(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Invalid credentials." } };
            }

            var authResult = _tokenService.GenerateTokens(user, request.IpAddress);

            // Revoke old refresh tokens and add new one
            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<RefreshToken>();
            }

            // Revoke any existing active refresh tokens for the user
            foreach (var oldRefreshToken in user.RefreshTokens.Where(rt => rt.IsActive))
            {
                oldRefreshToken.Revoked = DateTime.UtcNow;
                oldRefreshToken.RevokedByIp = request.IpAddress;
            }

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = authResult.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(authResult.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = request.IpAddress,
                UserId = user.Id
            });

            await _context.SaveChangesAsync(cancellationToken);

            return authResult;
        }
    }
}
