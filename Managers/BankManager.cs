
using AutoMapper;
using DataAccess.Entities;
using Interfaces.IManager;
using Interfaces.IRepository;
using Microsoft.Extensions.Logging;
using Models.DTOs;

namespace Managers
{
    public class BankManager : IBankManager
    {
        private readonly IGenericRepository<Bank> _bankRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BankManager> _logger;

        public BankManager(IGenericRepository<Bank> bankRepository, IMapper mapper, ILogger<BankManager> logger)
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> CreateBankAsync(CreateBankDto dto)
        {
            _logger.LogInformation("Started adding bank {bankName}", dto.Name);
            try
            {
                var bank = _mapper.Map<Bank>(dto);
                bank.IsActive = true;
                await _bankRepository.AddAsync(bank);
                _logger.LogInformation("Bank {bankName} added successfully", dto.Name);
                return new Result { Success = true, Message = "Added Successfully" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed adding bank {name}", dto.Name);
                return new Result { Success = false, Message = ex.Message };
            }
        }

        public async Task<Result> UpdateBankAsync(int bankId, UpdateBankDto dto)
        {
            var bank = await _bankRepository.GetByIdAsync(bankId);
            if (bank == null) return new Result { Success = false, Message = "Bank not found" };

            _mapper.Map(dto, bank);
           await _bankRepository.Update(bank);
            return new Result { Success = true, Message = "Updated Successfully" };
        }

        public async Task<Result> DeleteBankAsync(int bankId)
        {
            var bank = await _bankRepository.GetByIdAsync(bankId);
            if (bank == null) return new Result { Success = false, Message = "Bank not found" };

         await   _bankRepository.Delete(bank);
            return new Result { Success = true, Message = "Deleted Successfully" };
        }

        public async Task<Result<List<BankDto>>> GetAllBanksAsync()
        {
            var banks = await _bankRepository.GetAllAsync();
            var dtos = _mapper.Map<List<BankDto>>(banks);
            return new Result<List<BankDto>> { Success = true, Data = dtos };
        }

        public async Task<Result<BankDto>> GetBankByIdAsync(int bankId)
        {
            var bank = await _bankRepository.GetByIdAsync(bankId);
            if (bank == null) return new Result<BankDto> { Success = false, Message = "Bank not found" };

            var dto = _mapper.Map<BankDto>(bank);
            return new Result<BankDto> { Success = true, Data = dto };
        }

        public async Task<Result> ActivateBankAsync(int bankId)
        {
            var bank = await _bankRepository.GetByIdAsync(bankId);
            if (bank == null) return new Result { Success = false, Message = "Bank not found" };

            bank.IsActive = true;
           await _bankRepository.Update(bank);
            return new Result { Success = true, Message = "Bank Activated" };
        }

        public async Task<Result> DeactivateBankAsync(int bankId)
        {
            var bank = await _bankRepository.GetByIdAsync(bankId);
            if (bank == null) return new Result { Success = false, Message = "Bank not found" };

            bank.IsActive = false;
          await  _bankRepository.Update(bank);
            return new Result { Success = true, Message = "Bank Deactivated" };
        }
    }
}
