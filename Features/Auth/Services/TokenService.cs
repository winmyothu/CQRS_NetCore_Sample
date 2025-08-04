using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CQRSExample.Models;
using CQRSExample.Features.Auth.Models;

namespace CQRSExample.Features.Auth.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResult GenerateTokens(User user, string ipAddress)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var tokenExpirationMinutes = int.Parse(jwtSettings["TokenExpirationMinutes"]);
            var refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"]);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(tokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken(ipAddress);

            return new AuthResult
            {
                Succeeded = true,
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                AccessTokenExpirationMinutes = tokenExpirationMinutes,
                RefreshTokenExpirationDays = refreshTokenExpirationDays
            };
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var refreshTokenExpirationDays = int.Parse(jwtSettings["RefreshTokenExpirationDays"]);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                Expires = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }
    }
}
