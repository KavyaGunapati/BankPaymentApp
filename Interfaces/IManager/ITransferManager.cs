
using Models.DTOs;

namespace Interfaces.IManager
{
    public interface ITransferManager
    {
        Task<Result<PaymentIntentResponseDto>> CreateTransferIntentAsync(
            string userId, CreateTransferIntentDto dto);
        Task<Result<string>> ConfirmPayment(string paymentId, string paymentMethodid);
        Task<Result> MarkTransferFailedAsync(long transferId, string reason);
    }
}
