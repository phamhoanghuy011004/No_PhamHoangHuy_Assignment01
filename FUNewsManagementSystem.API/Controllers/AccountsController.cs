using FUNewsManagementSystem.API.DTOs.Accounts;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// GET /api/accounts
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAsync(cancellationToken);
            var result = accounts.Select(MapToResponseDto);
            return Ok(result);
        }

        /// <summary>
        /// GET /api/accounts/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(short id, CancellationToken cancellationToken)
        {
            var account = await _accountService.GetByIdAsync(id, cancellationToken);
            if (account == null)
                return NotFound(new { message = "Account not found." });

            return Ok(MapToResponseDto(account));
        }

        /// <summary>
        /// GET /api/accounts/search?keyword=xxx
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<AccountResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? keyword, CancellationToken cancellationToken)
        {
            var accounts = await _accountService.SearchAsync(keyword, cancellationToken);
            var result = accounts.Select(MapToResponseDto);
            return Ok(result);
        }

        /// <summary>
        /// POST /api/accounts
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(AccountResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] AccountCreateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = new SystemAccount
            {
                AccountID = dto.AccountID,
                AccountName = dto.AccountName,
                AccountEmail = dto.AccountEmail,
                AccountRole = dto.AccountRole,
                AccountPassword = dto.AccountPassword
            };

            var (success, message) = await _accountService.CreateAsync(account, cancellationToken);

            if (!success)
            {
                if (message.Contains("already exists"))
                    return Conflict(new { message });
                return BadRequest(new { message });
            }

            var created = await _accountService.GetByIdAsync(dto.AccountID, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = dto.AccountID }, MapToResponseDto(created!));
        }

        /// <summary>
        /// PUT /api/accounts/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(short id, [FromBody] AccountUpdateDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var account = new SystemAccount
            {
                AccountID = id,
                AccountName = dto.AccountName,
                AccountEmail = dto.AccountEmail,
                AccountRole = dto.AccountRole,
                AccountPassword = dto.AccountPassword
            };

            var (success, message) = await _accountService.UpdateAsync(account, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        /// <summary>
        /// DELETE /api/accounts/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(short id, CancellationToken cancellationToken)
        {
            var (success, message) = await _accountService.DeleteAsync(id, cancellationToken);

            if (!success)
            {
                if (message.Contains("not found"))
                    return NotFound(new { message });
                if (message.Contains("has created"))
                    return Conflict(new { message });
                return BadRequest(new { message });
            }

            return NoContent();
        }

        private static AccountResponseDto MapToResponseDto(SystemAccount account)
        {
            return new AccountResponseDto
            {
                AccountID = account.AccountID,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole
            };
        }
    }
}