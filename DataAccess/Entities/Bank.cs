using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{


    public class Bank
    {
        [Key] public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; } = null!;

        [StringLength(11, MinimumLength = 11)]
        public string? IFSC { get; set; } // optional for demo

        [StringLength(11)]
        public string? SwiftCode { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }

}
