using AutoMapper;
using DataAccess.Entities;
using Interfaces.IManager;
using Interfaces.IRepository;
using Microsoft.AspNetCore.Identity;
using Models.DTOs;
using Models.Enums;
using Entity = DataAccess.Entities;

namespace Managers
{
    public class TransferManager : ITransferManager
    {
        private readonly UserManager<Entity.ApplicationUser> _userManager;
        private readonly IGenericRepository<Entity.Transaction> _transactionRepo;
        private readonly IGenericRepository<Entity.Payment> _paymentRepo;
        private readonly IGenericRepository<Entity.Transfer> _transferRepo;
        private readonly IMapper _mapper;
        private readonly IPaymentManager _paymentManager;
        private readonly IGenericRepository<Entity.Account> _accountRepo;
        public TransferManager(IGenericRepository<Entity.Account> accountRepo, IPaymentManager paymentManager, UserManager<ApplicationUser> userManager, IGenericRepository<Transaction> transactionRepo,
            IGenericRepository<Entity.Transfer> tranferRepo,
            IGenericRepository<Entity.Payment> paymentRepo,
            IMapper mapper)
        {
            _userManager = userManager;
            _transactionRepo = transactionRepo;
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _transferRepo = tranferRepo;
            _accountRepo = accountRepo;
            _paymentManager = paymentManager;
        }

        public async Task<Result<string>> ConfirmPayment(string paymentId, string paymentMethodId)
        {
            try
            {
                // Confirm payment via Stripe (or your PaymentManager)
                var confirmResult = await _paymentManager.ConfirmPaymemnt(paymentId, paymentMethodId);
                if (confirmResult!="Succeeded")
                    return new Result<string> { Success = false, Message = "Payment confirmation failed." };

                var payment = (await _paymentRepo.GetAllAsync()).FirstOrDefault(p=>p.PaymentIntentId==paymentId);
                if (payment == null)
                    return new Result<string> { Success = false, Message = "Payment not found." };

                var transfer = await _transferRepo.GetByIdAsync(payment.TransferId);
                if (transfer == null)
                    return new Result<string> { Success = false, Message = "Transfer not found." };

                var fromAccount = await _accountRepo.GetByIdAsync(transfer.FromAccountId);
                var toAccount = await _accountRepo.GetByIdAsync(transfer.ToAccountId);

                if (fromAccount == null || toAccount == null)
                    return new Result<string> { Success = false, Message = "Accounts not found." };

           
                fromAccount.Balance -= transfer.Amount;
                toAccount.Balance += transfer.Amount;

              await  _accountRepo.Update(fromAccount);
               await _accountRepo.Update(toAccount);

       
                transfer.Status = TransferStatus.Completed;
                payment.Status = PaymentStatus.Succeeded;

              await  _transferRepo.Update(transfer);
              await  _paymentRepo.Update(payment);

              
                var debitTx = new Transaction
                {
                    AccountId = fromAccount.Id,
                    Type = TransactionType.Debit,
                    Amount = transfer.Amount,
                    Currency = transfer.Currency,
                    Reference = payment.PaymentIntentId,
                    Description = $"Transfer to {toAccount.AccountNumber}"
                };

                var creditTx = new Transaction
                {
                    AccountId = toAccount.Id,
                    Type = TransactionType.Credit,
                    Amount = transfer.Amount,
                    Currency = transfer.Currency,
                    Reference = payment.PaymentIntentId,
                    Description = $"Transfer from {fromAccount.AccountNumber}"
                };

                await _transactionRepo.AddAsync(debitTx);
                await _transactionRepo.AddAsync(creditTx);

                return new Result<string> { Success = true, Data = "Payment confirmed and transfer completed." };
            }
            catch (Exception ex)
            {
                return new Result<string> { Success = false, Message = ex.Message };
            }
        }


        public async Task<Result<PaymentIntentResponseDto>> CreateTransferIntentAsync(string userId, CreateTransferIntentDto dto)
        {
            try
            {
                var userExist = await _userManager.FindByIdAsync(userId);
                if (userExist == null) return new Result<PaymentIntentResponseDto> { Success = false, Message = "User Not found" };

                var from = await _accountRepo.GetByIdAsync(dto.FromAccountId);
                var to = await _accountRepo.GetByIdAsync(dto.ToAccountId);

                if (from is null || to is null) return new Result<PaymentIntentResponseDto> { Success = false, Message = "Account not found." };
                if (!from.IsActive || !to.IsActive) return new Result<PaymentIntentResponseDto> { Success = false, Message = "Inactive account." };

                if (from.UserId != userId) return new Result<PaymentIntentResponseDto> { Message = "FromAccount does not belong to caller." };
                if ((from.Currency) != dto.Currency || to.Currency != dto.Currency)
                    return new Result<PaymentIntentResponseDto> { Message = "Currency mismatch." };
                if (dto.Amount <= 0m) return new Result<PaymentIntentResponseDto> { Message = "Amount must be greater than zero." };

                if (from.Id == to.Id) return new Result<PaymentIntentResponseDto> { Message = "Cannot transfer to the same account." };

                var transfer = new Transfer
                {
                    FromAccountId = from.Id,
                    ToAccountId = to.Id,
                    Amount = dto.Amount,
                    Currency = dto.Currency,
                    Status = TransferStatus.Pending,
                    Reference = dto.Reference
                };
                await _transferRepo.AddAsync(transfer);

                (string intentId, string clientSecret) = await _paymentManager.CreatePaymentIntentAsync(transfer.Id, dto.Currency, dto.Amount);

                return new Result<PaymentIntentResponseDto>
                {
                    Success = true,
                    Data = new PaymentIntentResponseDto
                    {
                        TransferId = transfer.Id,
                        PaymentIntentId = intentId,
                        ClientSecret = clientSecret,
                        Amount = dto.Amount,
                        Currency = dto.Currency
                    }
                };
            }
            catch (Exception ex)
            {
                return new Result<PaymentIntentResponseDto> { Success = false, Message = ex.Message };
            }

        }

        public Task<Result> MarkTransferFailedAsync(long transferId, string reason)
        {
            throw new NotImplementedException();
        }
    }
}
