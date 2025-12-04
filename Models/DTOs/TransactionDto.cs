
namespace Models.DTOs
{
    public class TransactionDto
    {
        public long Id { get; set; }
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public Currency Currency { get; set; }
        public string? Reference { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
