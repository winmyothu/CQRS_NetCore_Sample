using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CQRSExample.Data;
using CQRSExample.Features.Auth.Commands;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CQRSExample.Features.Auth.Handlers
{
    public class RevokeRefreshTokenHandler : IRequestHandler<RevokeRefreshTokenCommand, bool>
    {
        private readonly AppDbContext _context;

        public RevokeRefreshTokenHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == request.RefreshToken, cancellationToken);

            if (refreshToken == null)
            {
                return false; // Token not found
            }

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = request.IpAddress;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
