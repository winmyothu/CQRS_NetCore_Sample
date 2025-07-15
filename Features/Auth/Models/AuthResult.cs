namespace CQRSExample.Features.Auth.Models
{
    public class AuthResult
    {
        public bool Succeeded { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public string[] Errors { get; set; }
    }
}
