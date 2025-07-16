using System;
using System.ComponentModel.DataAnnotations;

namespace CQRSExample.Models
{
    public class PaymentTransaction
    {
        public int Id { get; set; }

        [Required]
        public int GuestRegistrationId { get; set; }
        public required GuestRegistration GuestRegistration { get; set; } // Navigation property

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(50)]
        public required string PaymentMethod { get; set; } // e.g., "Credit Card", "Bank Transfer", "Cash"

        [StringLength(255)]
        public required string TransactionReference { get; set; } // e.g., Stripe charge ID, bank transaction ID

        [StringLength(50)]
        public string Status { get; set; } = "Completed"; // e.g., "Completed", "Pending", "Failed"
    }
}
