using DataAccess.Entities;
using DataAccess.Enums;
using Interfaces.IManager;
using Interfaces.IRepository;
using DTO= Models.DTOs;
using Stripe;
using Microsoft.Extensions.Configuration;

namespace Managers
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IConfiguration _configuration;
        public PaymentManager(IGenericRepository<Payment> paymentRepo,IConfiguration configuration)
        {
            _paymentRepo = paymentRepo;
            _configuration = configuration;
        }
        public Task<string> ConfirmPaymemnt(string paymentId, string paymentMethodid)
        {
            throw new NotImplementedException();
        }
        public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(int tarnsferId, DTO.Currency currency, decimal amount)
        {
            var minor = (long)(amount * 100);
            var options = new PaymentIntentCreateOptions
            {
                Amount = minor,
                Currency = currency.ToString().ToLower(),
                PaymentMethodTypes = new List<string> { "card" },
                Metadata=new Dictionary<string, string> { { "transferId",tarnsferId.ToString() } }
            };
            var service = new PaymentIntentService();
            var intent=await service.CreateAsync(options);
            var payment = new Payment
            {
                TransferId = tarnsferId,
                PaymentIntentId = intent.Id,
                Status = PaymentStatus.Pending,
                Amount=amount
            };
            await _paymentRepo.AddAsync(payment);
            return  (intent.Id, intent.ClientSecret);
        }

        public Task HandleWebhookAsync(string json, string signatureHeader)
        {
            var webhookSecret = _configuration[""]
            try
            {
               
            }
        }
    }
}
