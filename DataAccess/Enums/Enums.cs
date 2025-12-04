namespace DataAccess.Enums
{

    public enum Currency { INR = 1, USD = 2 }
    public enum TransactionType { Credit = 1, Debit = 2 }
    public enum TransferStatus
    {
        Pending = 1, Completed = 2, Failed = 3
    }
    public enum PaymentStatus { Pending = 1, Failed = 2 ,Succeeded=3}
    }
