
using Interfaces.IManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using System.Security.Claims;

namespace BankPaymentAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    [Authorize] 
    public class AccountsController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public AccountsController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }
        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            var userId = GetUserId();
            var result = await _accountManager.CreateAccountAsync(userId, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

       
        [HttpPut("{accountId:int}")]
        public async Task<IActionResult> UpdateAccount(int accountId, [FromBody] UpdateAccountDto dto)
        {
            var userId = GetUserId();
            var result = await _accountManager.UpdateAccountAsync(accountId, dto, userId);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

      
        [HttpDelete("{accountId:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var result = await _accountManager.DeleteAccountAsync(accountId);
            return result.Success ? Ok(result) : NotFound(result);
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts()
        {
            var result = await _accountManager.GetAllAccountsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

      
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccounts()
        {
            var userId = GetUserId();
            var result = await _accountManager.GetAccountsForUserAsync(userId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("{accountId:int}")]
        public async Task<IActionResult> GetAccountById(int accountId)
        {
            var userId = GetUserId();
            var result = await _accountManager.GetAccountByIdAsync(accountId, userId);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

        [HttpGet("{accountId:int}/transactions")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var userId = GetUserId();
            var result = await _accountManager.GetTransactionsAsync(accountId, userId);
            return result.Success ? Ok(result) : Unauthorized(result);
        }

      
        [HttpPut("{accountId:int}/status")]
        public async Task<IActionResult> ToggleStatus(int accountId, [FromQuery] bool isActive)
        {
            var userId = GetUserId();
            var result = await _accountManager.ToggleAccountStatusAsync(accountId, isActive, userId);
            return result.Success ? Ok(result) : Unauthorized(result);
        }
    }
}

