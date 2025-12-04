
using Models.DTOs;

namespace Interfaces.IManager
{
    public interface IAccountManager
    {
        Task<Result> CreateAccountAsync(string userId, CreateAccountDto dto);
        Task<Result> UpdateAccountAsync(int accountId, UpdateAccountDto dto, string userIdOrAdmin);
        Task<Result> DeleteAccountAsync(int accountId); 

        Task<Result<List<AccountDto>>> GetAllAccountsAsync();
        Task<Result<List<AccountDto>>> GetAccountsForUserAsync(string userId);
        Task<Result<AccountDto>> GetAccountByIdAsync(int accountId, string userIdOrAdmin);

        
        Task<Result<List<TransactionDto>>> GetTransactionsAsync(
            int accountId,  string userIdOrAdmin);

        Task<Result> ToggleAccountStatusAsync(int accountId, bool isActive, string userIdOrAdmin);
    }
}
