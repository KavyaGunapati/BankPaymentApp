namespace Models.DTOs
{
    public class CreateTransferIntentDto
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string? Reference { get; set; }
    }

    public class PaymentIntentResponseDto
    {
        public long TransferId { get; set; }
        public string PaymentIntentId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
    }

}
