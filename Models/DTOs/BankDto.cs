namespace Models.DTOs
{

    public class CreateBankDto
    {
        public string Name { get; set; } = null!;
        public string? IFSC { get; set; }
        public string? SwiftCode { get; set; }
    }

    public class UpdateBankDto : CreateBankDto
    {
        public bool IsActive { get; set; } = true;
    }

    public class BankDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? IFSC { get; set; }
        public string? SwiftCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateAccountDto
    {
        public int BankId { get; set; }
        public Currency Currency { get; set; }
    }

    public class UpdateAccountDto
    {
        public bool IsActive { get; set; }
    }

    public class AccountDto
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public string AccountNumber { get; set; } = null!;
        public Currency Currency { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
    }

}
