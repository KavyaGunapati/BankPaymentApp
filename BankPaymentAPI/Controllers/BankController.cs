
using Interfaces.IManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.DTOs;
using System;

namespace BankPaymentAPI.Controllers
{
    [ApiController]
    [Route("api/banks")]
    [Authorize(Roles = "Admin")] 
    public class BanksController : ControllerBase
    {
        private readonly IBankManager _bankManager;
        private readonly ILogger<BanksController> _logger;

        public BanksController(IBankManager bankManager, ILogger<BanksController> logger)
        {
            _bankManager = bankManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBankDto dto)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier
                });

                _logger.LogInformation("Create bank request received.");

                var result = await _bankManager.CreateBankAsync(dto);
                return result.Success?Ok(result):BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bank.");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{bankId:int}")]
        public async Task<IActionResult> Update([FromRoute] int bankId, [FromBody] UpdateBankDto dto)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier,
                    ["BankId"] = bankId
                });

                _logger.LogInformation("Update bank request for {BankId}.", bankId);

                var result = await _bankManager.UpdateBankAsync(bankId, dto);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bank {BankId}.", bankId);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{bankId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int bankId)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier,
                    ["BankId"] = bankId
                });

                _logger.LogInformation("Delete bank request for {BankId}.", bankId);

                var result = await _bankManager.DeleteBankAsync(bankId);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bank {BankId}.", bankId);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier
                });

                _logger.LogInformation("Get all banks request.");

                var result = await _bankManager.GetAllBanksAsync();
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all banks.");
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{bankId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int bankId)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier,
                    ["BankId"] = bankId
                });

                _logger.LogInformation("Get bank by id request: {BankId}.", bankId);

                var result = await _bankManager.GetBankByIdAsync(bankId);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching bank {BankId}.", bankId);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("{bankId:int}/activate")]
        public async Task<IActionResult> Activate([FromRoute] int bankId)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier,
                    ["BankId"] = bankId
                });

                _logger.LogInformation("Activate bank request: {BankId}.", bankId);

                var result = await _bankManager.ActivateBankAsync(bankId);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error activating bank {BankId}.", bankId);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{bankId:int}/deactivate")]
        public async Task<IActionResult> Deactivate([FromRoute] int bankId)
        {
            try
            {
                using var scope = _logger.BeginScope(new Dictionary<string, object?>
                {
                    ["CorrelationId"] = HttpContext.TraceIdentifier,
                    ["BankId"] = bankId
                });

                _logger.LogInformation("Deactivate bank request: {BankId}.", bankId);

                var result = await _bankManager.DeactivateBankAsync(bankId);
                return result.Success ? Ok(result) : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deactivating bank {BankId}.", bankId);
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
