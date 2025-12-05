using DataAccess.Entities;
using Interfaces.IManager;
using Interfaces.IRepository;
using Microsoft.Extensions.Configuration;
using Models.Enums;
using Stripe;

namespace Managers
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IConfiguration _configuration;
        public PaymentManager(IGenericRepository<Payment> paymentRepo, IConfiguration configuration)
        {
            _paymentRepo = paymentRepo;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }
        public async Task<string> ConfirmPaymemnt(string paymentId, string paymentMethodid)
        {
            var service = new PaymentIntentService();
            var confirmOptions = new PaymentIntentConfirmOptions
            {
                PaymentMethod = paymentMethodid,
            };
            var intent = await service.ConfirmAsync(paymentId, confirmOptions);
            return intent.Status;
        }
        public async Task<(string PaymentIntentId, string ClientSecret)> CreatePaymentIntentAsync(long tarnsferId, Currency currency, decimal amount)
        {
            var minor = (long)(amount * 100);
            var options = new PaymentIntentCreateOptions
            {
                Amount = minor,
                Currency = currency.ToString().ToLower(),
                PaymentMethodTypes = new List<string> { "card" },
                Metadata = new Dictionary<string, string> { { "transferId", tarnsferId.ToString() } }
            };
            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);
            var payment = new Payment
            {
                TransferId = tarnsferId,
                PaymentIntentId = intent.Id,
                Status = PaymentStatus.Pending,
                Amount = amount
            };
            await _paymentRepo.AddAsync(payment);
            return (intent.Id, intent.ClientSecret);
        }

        public async Task HandleWebhookAsync(string json, string signatureHeader)
        {
            var webhookSecret = _configuration["Stripe:WebhookSecret"];
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, webhookSecret);

            }
            catch (Exception ex)
            {


                throw new Exception(ex.Message);

            }
            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var success = stripeEvent.Data.Object as PaymentIntent;
                if (success != null)
                {
                    var payment = (await _paymentRepo.GetAllAsync()).FirstOrDefault(p => p.PaymentIntentId == success.Id);
                    if (payment != null)
                    {
                        payment.Status = PaymentStatus.Succeeded;
                        await _paymentRepo.Update(payment);
                    }
                }
            }
            else if (stripeEvent.Type == "payment_failed.payment_failed")
            {
                var failed = stripeEvent.Data.Object as PaymentIntent;
                if (failed != null)
                {
                    var payment = (await _paymentRepo.GetAllAsync()).FirstOrDefault(p => p.PaymentIntentId == failed.Id);
                    if (payment != null)
                    {
                        payment.Status = PaymentStatus.Failed;
                        await _paymentRepo.Update(payment);
                    }
                }
            }
        }
    }
}
