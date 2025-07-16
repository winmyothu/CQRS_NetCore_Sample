using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Features.Auth.Commands;
using CQRSExample.Features.Auth.Models;
using CQRSExample.Features.Auth.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CQRSExample.Models; // Ensure this is present

namespace CQRSExample.Features.Auth.Handlers
{
    public class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenCommand, AuthResult>
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public RefreshAccessTokenHandler(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .Include(u => u.RefreshTokens)
                .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == request.RefreshToken), cancellationToken);

            if (user == null)
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Invalid token." } };
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == request.RefreshToken);

            if (refreshToken.IsExpired)
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Refresh token has expired." } };
            }

            // Revoke the old token
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IpAddress;

            // Generate new tokens for the user
            var newAuthResult = _tokenService.GenerateTokens(user, request.IpAddress);

            // Replace old refresh token with a new one
            var newRefreshToken = new RefreshToken
            {
                Token = newAuthResult.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(newAuthResult.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = request.IpAddress
            };
            user.RefreshTokens.Add(newRefreshToken);

            // Revoke the old token and set the replacement
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IpAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            await _context.SaveChangesAsync(cancellationToken);

            return newAuthResult;
        }
    }
}
