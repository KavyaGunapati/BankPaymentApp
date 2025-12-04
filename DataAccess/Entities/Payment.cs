using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Payment
    {
        [Key] public long Id { get; set; }

        [Required] public long TransferId { get; set; }
        public Transfer Transfer { get; set; } = null!;

        [Required, StringLength(100)]
        public string PaymentIntentId { get; set; } = null!; // unique

        [Required] public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        [Range(0.01, double.MaxValue)] public decimal Amount { get; set; }
        [Required] public Currency Currency { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
