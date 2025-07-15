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
            var refreshToken = await _context.RefreshTokens
                .Include(rt => rt.User)
                .SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (refreshToken == null || !refreshToken.IsActive)
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Invalid refresh token." } };
            }

            // Revoke old token
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IpAddress;

            // Generate new tokens
            var newAuthResult = _tokenService.GenerateTokens(refreshToken.User, request.IpAddress);

            // Add new refresh token
            refreshToken.User.RefreshTokens.Add(new RefreshToken
            {
                Token = newAuthResult.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(newAuthResult.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = request.IpAddress,
                UserId = refreshToken.User.Id,
                ReplacedByToken = newAuthResult.RefreshToken // Link to the new token
            });

            await _context.SaveChangesAsync(cancellationToken);

            return newAuthResult;
        }
    }
}
