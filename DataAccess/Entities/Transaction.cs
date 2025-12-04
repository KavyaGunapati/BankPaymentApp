using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Transaction
    {
        [Key] 
        public long Id { get; set; }

        [Required] 
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;

        [Required] 
        public TransactionType Type { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required] 
        public Currency Currency { get; set; }

        [StringLength(100)] 
        public string? Reference { get; set; }
        [StringLength(200)] 
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}