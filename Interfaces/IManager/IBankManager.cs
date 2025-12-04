
using Models.DTOs;

namespace Interfaces.IManager
{
    public interface IBankManager
    {
        Task<Result> CreateBankAsync(CreateBankDto dto);
        Task<Result> UpdateBankAsync(int bankId, UpdateBankDto dto);
        Task<Result> DeleteBankAsync(int bankId); // soft delete recommended
        Task<Result<List<BankDto>>> GetAllBanksAsync();
        Task<Result<BankDto>> GetBankByIdAsync(int bankId);
        Task<Result> ActivateBankAsync(int bankId);
        Task<Result> DeactivateBankAsync(int bankId);
    }
}
