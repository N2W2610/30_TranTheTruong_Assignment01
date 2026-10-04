using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface IAccountService
    {
        AuthenticatedUser? Authenticate(string email, string password);
        List<SystemAccount> GetAll();
        SystemAccount? Get(short id);
        ServiceResult Create(SystemAccount account);
        ServiceResult Update(short id, SystemAccount account);
        ServiceResult Delete(short id);
        ProfileResponse? GetProfile(short id);
        ServiceResult UpdateProfile(short id, ProfileRequest request);
    }

    public class AccountService : IAccountService
    {
        private readonly ISystemAccountRepository _repo;
        private readonly IConfiguration _config;

        public AccountService(ISystemAccountRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public AuthenticatedUser? Authenticate(string email, string password)
        {
            email = email.Trim();

            // The default admin account lives in appsettings.json (not in the database).
            var adminEmail = _config["AdminAccount:Email"];
            var adminPassword = _config["AdminAccount:Password"];
            if (!string.IsNullOrEmpty(adminEmail)
                && string.Equals(email, adminEmail, StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, adminPassword, StringComparison.Ordinal))
            {
                return new AuthenticatedUser(0, "Administrator", adminEmail, "Admin");
            }

            var account = _repo.Login(email, password);
            if (account == null) return null;

            var role = account.AccountRole == 1 ? "Staff" : "Lecturer";
            return new AuthenticatedUser(account.AccountId, account.AccountName ?? string.Empty, account.AccountEmail ?? string.Empty, role);
        }

        public List<SystemAccount> GetAll()
        {
            var accounts = _repo.GetAll();
            accounts.ForEach(a => a.AccountPassword = null); // never expose passwords
            return accounts;
        }

        public SystemAccount? Get(short id)
        {
            var account = _repo.GetById(id);
            if (account != null) account.AccountPassword = null;
            return account;
        }

        public ServiceResult Create(SystemAccount account)
        {
            if (string.IsNullOrWhiteSpace(account.AccountPassword))
                return ServiceResult.Fail("Password is required.");

            account.AccountName = account.AccountName!.Trim();
            account.AccountEmail = account.AccountEmail!.Trim();

            if (IsAdminEmail(account.AccountEmail) || _repo.EmailExists(account.AccountEmail, 0))
                return ServiceResult.Fail("This email is already in use.");

            _repo.Add(account);
            return ServiceResult.Ok();
        }

        public ServiceResult Update(short id, SystemAccount account)
        {
            if (_repo.GetById(id) == null) return ServiceResult.Missing("Account not found.");

            account.AccountId = id;
            account.AccountName = account.AccountName!.Trim();
            account.AccountEmail = account.AccountEmail!.Trim();

            if (IsAdminEmail(account.AccountEmail) || _repo.EmailExists(account.AccountEmail, id))
                return ServiceResult.Fail("This email is already in use.");

            return _repo.Update(account) ? ServiceResult.Ok() : ServiceResult.Missing("Account not found.");
        }

        public ServiceResult Delete(short id)
        {
            if (_repo.GetById(id) == null) return ServiceResult.Missing("Account not found.");

            if (_repo.HasCreatedNews(id))
                return ServiceResult.Fail("This account has already created news articles and cannot be deleted.");

            return _repo.Delete(id) ? ServiceResult.Ok() : ServiceResult.Missing("Account not found.");
        }

        public ProfileResponse? GetProfile(short id)
        {
            var account = _repo.GetById(id);
            return account == null
                ? null
                : new ProfileResponse { AccountId = account.AccountId, AccountName = account.AccountName, AccountEmail = account.AccountEmail };
        }

        public ServiceResult UpdateProfile(short id, ProfileRequest request)
        {
            var account = _repo.GetById(id);
            if (account == null) return ServiceResult.Missing("Account not found.");

            var email = request.AccountEmail!.Trim();
            if (IsAdminEmail(email) || _repo.EmailExists(email, id))
                return ServiceResult.Fail("This email is already in use.");

            account.AccountName = request.AccountName!.Trim();
            account.AccountEmail = email;
            account.AccountPassword = string.IsNullOrWhiteSpace(request.NewPassword) ? null : request.NewPassword;

            return _repo.Update(account) ? ServiceResult.Ok() : ServiceResult.Missing("Account not found.");
        }

        private bool IsAdminEmail(string email) =>
            string.Equals(email, _config["AdminAccount:Email"], StringComparison.OrdinalIgnoreCase);
    }
}
