using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{

    public class Transfer
    {
        [Key] public long Id { get; set; }

        [Required]
        public int FromAccountId { get; set; }
        public Account FromAccount { get; set; } = null!;

        [Required]
        public int ToAccountId { get; set; }
        public Account ToAccount { get; set; } = null!;

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required] 
        public Currency Currency { get; set; }

        [Required]
        public TransferStatus Status { get; set; } = TransferStatus.Pending;

        [StringLength(100)] 
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Payment Payment { get; set; } = null!;
    }
    }
