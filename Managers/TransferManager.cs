using AutoMapper;
using DataAccess.Entities;
using Interfaces.IManager;
using Interfaces.IRepository;
using Microsoft.AspNetCore.Identity;
using Models.DTOs;

namespace Managers
{
    public class TransferManager : ITransferManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<Transaction> _transactionRepo;
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly IGenericRepository<Transfer> _transferRepo;
        private readonly IMapper _mapper;
        public TransferManager(UserManager<ApplicationUser> userManager,IGenericRepository<Transaction> transactionRepo,
            IGenericRepository<Transfer> tranferRepo,
            IGenericRepository<Payment> paymentRepo,
            IMapper mapper)
        {
            _userManager = userManager;
            _transactionRepo = transactionRepo;
            _paymentRepo = paymentRepo;
            _mapper = mapper;
            _transferRepo=tranferRepo;
        }
        public Task<Result<string>> ConfirmPayment(string paymentId, string paymentMethodid)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<PaymentIntentResponseDto>> CreateTransferIntentAsync(string userId, CreateTransferIntentDto dto)
        {
            try
            {
                var userExist = await _userManager.FindByIdAsync(userId);
                if (userExist == null) return new Result<PaymentIntentResponseDto> { Success = false, Message = "User Not found" };

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
