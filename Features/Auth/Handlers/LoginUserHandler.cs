using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Features.Auth.Queries;
using CQRSExample.Features.Auth.Models;
using CQRSExample.Features.Auth.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using CQRSExample.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CQRSExample.Features.Auth.Handlers
{
    public class LoginUserHandler : IRequestHandler<LoginUserQuery, AuthResult>
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginUserHandler(AppDbContext context, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResult> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResult { Succeeded = false, Errors = new[] { "Invalid credentials." } };
            }

            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var authResult = _tokenService.GenerateTokens(user, ipAddress);

            // Revoke old refresh tokens and add new one
            if (user.RefreshTokens == null)
            {
                user.RefreshTokens = new List<RefreshToken>();
            }

            // Revoke any existing active refresh tokens for the user
            foreach (var oldRefreshToken in user.RefreshTokens.Where(rt => rt.IsActive))
            {
                oldRefreshToken.Revoked = DateTime.UtcNow;
                oldRefreshToken.RevokedByIp = ipAddress;
            }

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = authResult.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(authResult.RefreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserId = user.Id // No previous token to replace
            });

            await _context.SaveChangesAsync(cancellationToken);

            return authResult;
        }
    }
}
