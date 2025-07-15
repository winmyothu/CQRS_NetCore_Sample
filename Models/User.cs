using System.Collections.Generic;

namespace CQRSExample.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Store hashed passwords
        public string Role { get; set; } = "User"; // e.g., Admin, User

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
