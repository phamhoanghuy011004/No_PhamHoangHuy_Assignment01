using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using FUNewsManagementSystem.API.Services.Interfaces;

namespace FUNewsManagementSystem.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _accountRepository;

        public AccountService(ISystemAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IEnumerable<SystemAccount>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _accountRepository.GetAllAsync(cancellationToken);
        }

        public async Task<SystemAccount?> GetByIdAsync(short id, CancellationToken cancellationToken = default)
        {
            return await _accountRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _accountRepository.GetByEmailAsync(email, cancellationToken);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _accountRepository.SearchAsync(keyword, cancellationToken);
        }

        public async Task<(bool Success, string Message)> CreateAsync(SystemAccount account, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(account.AccountEmail))
            {
                return (false, "Email is required.");
            }

            var existing = await _accountRepository.GetByEmailAsync(account.AccountEmail, cancellationToken);
            if (existing != null)
            {
                return (false, "Email already exists.");
            }

            await _accountRepository.AddAsync(account, cancellationToken);
            await _accountRepository.SaveChangesAsync(cancellationToken);

            return (true, "Account created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(SystemAccount account, CancellationToken cancellationToken = default)
        {
            var existing = await _accountRepository.GetByIdAsync(account.AccountID, cancellationToken);
            if (existing == null)
            {
                return (false, "Account not found.");
            }

            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            existing.AccountPassword = account.AccountPassword;

            _accountRepository.Update(existing);
            await _accountRepository.SaveChangesAsync(cancellationToken);

            return (true, "Account updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(short id, CancellationToken cancellationToken = default)
        {
            var account = await _accountRepository.GetByIdAsync(id, cancellationToken);
            if (account == null)
            {
                return (false, "Account not found.");
            }

            // Business rule: Cannot delete if account has created any news
            var hasNews = await _accountRepository.HasCreatedAnyNewsAsync(id, cancellationToken);
            if (hasNews)
            {
                return (false, "Cannot delete this account because it has created news articles.");
            }

            _accountRepository.Delete(account);
            await _accountRepository.SaveChangesAsync(cancellationToken);

            return (true, "Account deleted successfully.");
        }
    }
}