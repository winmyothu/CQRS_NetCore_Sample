using System;
using System.Collections.Generic;

namespace CQRSExample.Models
{
    /// <summary>
    /// Represents a guest registration form submission.
    /// </summary>
    public class GuestRegistration
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string PassportNumber { get; set; }
        public required string Nationality { get; set; }
        public required string Nrc { get; set; }
        public required string CurrentAddress { get; set; }
        public required string PermanentAddress { get; set; }

        /// <summary>
        /// Stores the URLs of the attached files, serialized as a JSON string.
        /// </summary>
        public required string AttachedFileUrls { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
