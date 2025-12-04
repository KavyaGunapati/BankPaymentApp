using DataAccess.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Account
    {
        [Key] public int Id { get; set; }

        [Required] 
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required] 
        public int BankId { get; set; }
        public Bank Bank { get; set; } = null!;

        [Required, StringLength(20)]
        public string AccountNumber { get; set; } = null!; // unique

        [Required] public Currency Currency { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; } = 0m;

        public bool IsActive { get; set; } = true;

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();


    }