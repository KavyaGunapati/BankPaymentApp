
using AutoMapper;
using DataAccess.Entities;
using Interfaces.IManager;
using Interfaces.IRepository;
using Microsoft.Extensions.Logging;
using Models.DTOs;

namespace Managers
{
    public class AccountManager : IAccountManager
    {
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Transaction> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountManager> _logger;

        public AccountManager(IGenericRepository<Account> accountRepository,
                              IGenericRepository<Transaction> transactionRepository,
                              IMapper mapper,
                              ILogger<AccountManager> logger)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> CreateAccountAsync(string userId, CreateAccountDto dto)
        {
            try
            {
                var account = _mapper.Map<Account>(dto);
                account.UserId = userId;
                account.AccountNumber = $"AC{Random.Shared.Next(100000000, 999999999)}";
                account.Balance = 10000;
                account.IsActive = true;

                await _accountRepository.AddAsync(account);

                _logger.LogInformation("Account created for user {userId}", userId);
                return new Result { Success = true, Message = "Account Created Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create account for user {userId}", userId);
                return new Result { Success = false, Message = ex.Message };
            }
        }

        public async Task<Result> UpdateAccountAsync(int accountId, UpdateAccountDto dto, string userIdOrAdmin)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return new Result { Success = false, Message = "Account not found" };

            if (account.UserId != userIdOrAdmin) return new Result { Success = false, Message = "Unauthorized" };

            account.IsActive = dto.IsActive;
            await _accountRepository.Update(account);
            return new Result { Success = true, Message = "Account Updated Successfully" };
        }

        public async Task<Result> DeleteAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return new Result { Success = false, Message = "Account not found" };

           await _accountRepository.Delete(account);
            return new Result { Success = true, Message = "Account Deleted Successfully" };
        }

        public async Task<Result<List<AccountDto>>> GetAllAccountsAsync()
        {
            var accounts = await _accountRepository.GetAllAsync();
            var dtos = _mapper.Map<List<AccountDto>>(accounts);
            return new Result<List<AccountDto>> { Success = true, Data = dtos };
        }

        public async Task<Result<List<AccountDto>>> GetAccountsForUserAsync(string userId)
        {
            var accounts = (await _accountRepository.GetAllAsync()).Where(a => a.UserId == userId).ToList();
            var dtos = _mapper.Map<List<AccountDto>>(accounts);
            return new Result<List<AccountDto>> { Success = true, Data = dtos };
        }

        public async Task<Result<AccountDto>> GetAccountByIdAsync(int accountId, string userIdOrAdmin)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return new Result<AccountDto> { Success = false, Message = "Account not found" };

            if (account.UserId != userIdOrAdmin) return new Result<AccountDto> { Success = false, Message = "Unauthorized" };

            var dto = _mapper.Map<AccountDto>(account);
            return new Result<AccountDto> { Success = true, Data = dto };
        }

        public async Task<Result<List<TransactionDto>>> GetTransactionsAsync(int accountId,  string userIdOrAdmin)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return new Result<List<TransactionDto>> { Success = false, Message = "Account not found" };

            if (account.UserId != userIdOrAdmin) return new Result<List<TransactionDto>> { Success = false, Message = "Unauthorized" };

            var transactions = await _transactionRepository.GetAllAsync();
            var filtered = transactions.Where(t => t.AccountId == accountId).ToList();
            var dtos = _mapper.Map<List<TransactionDto>>(filtered);
            return new Result<List<TransactionDto>> { Success = true, Data = dtos };
        }

        public async Task<Result> ToggleAccountStatusAsync(int accountId, bool isActive, string userIdOrAdmin)
        {
            var account = await _accountRepository.GetByIdAsync(accountId);
            if (account == null) return new Result { Success = false, Message = "Account not found" };

            if (account.UserId != userIdOrAdmin) return new Result { Success = false, Message = "Unauthorized" };

            account.IsActive = isActive;
         await   _accountRepository.Update(account);
            return new Result { Success = true, Message = "Account status updated" };
        }
    }
}
