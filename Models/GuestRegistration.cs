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
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string Nrc { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }

        /// <summary>
        /// Stores the URLs of the attached files, serialized as a JSON string.
        /// </summary>
        public string AttachedFileUrls { get; set; }
    }
}
