using Models.DTOs;

namespace Interfaces.IManager
{
    public interface IPaymentManager
    {
        Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(int tarnsferId, Currency currency, decimal amount);
        Task<string> ConfirmPaymemnt(string paymentId, string paymentMethodid);
        Task HandleWebhookAsync(string json, string signatureHeader);

    }
}
