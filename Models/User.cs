using System.Collections.Generic;

namespace CQRSExample.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; } // Store hashed passwords
        public string Role { get; set; } = "User"; // e.g., Admin, User

        public required ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
