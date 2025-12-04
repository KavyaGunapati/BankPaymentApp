namespace Models.DTOs
{

    public class RegisterDto
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "Customer"; // default role
    }

    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
    }

    public class RefreshRequestDto
    {
        public string RefreshToken { get; set; } = "";
    }

    public class CreateTransferDto
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string? Reference { get; set; }
    }

    public class TransferResultDto
    {
        public long TransferId { get; set; }
        public TransferStatus Status { get; set; }
        public decimal FromBalance { get; set; }
        public decimal ToBalance { get; set; }
    }
    public enum Currency { INR = 1, USD = 2 }
    public enum TransactionType { Credit = 1, Debit = 2 }
    public enum TransferStatus
    {
        Pending = 1, Completed = 2, Failed = 3
    }
}
