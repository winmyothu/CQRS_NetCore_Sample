using CQRSExample.Models;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Services
{
    public interface ITokenService
    {
        AuthResult GenerateTokens(User user, string ipAddress);
        RefreshToken GenerateRefreshToken(string ipAddress);
    }
}
