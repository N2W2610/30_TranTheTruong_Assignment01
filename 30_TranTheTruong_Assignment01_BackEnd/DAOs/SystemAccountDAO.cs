using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.DAOs
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? _instance;
        private static readonly object _lock = new();

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new SystemAccountDAO();
                }
            }
        }

        public List<SystemAccount> GetAll()
        {
            using var db = new FunewsManagementContext();
            return db.SystemAccounts.OrderBy(a => a.AccountId).ToList();
        }

        public SystemAccount? GetById(short id)
        {
            using var db = new FunewsManagementContext();
            return db.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
        }

        public SystemAccount? Login(string email, string password)
        {
            using var db = new FunewsManagementContext();
            var account = db.SystemAccounts.FirstOrDefault(a => a.AccountEmail == email);
            // SQL Server collation is case-insensitive, so compare the password in C# (case-sensitive).
            return account != null && string.Equals(account.AccountPassword, password, StringComparison.Ordinal)
                ? account
                : null;
        }

        public bool EmailExists(string email, short exceptId)
        {
            using var db = new FunewsManagementContext();
            return db.SystemAccounts.Any(a => a.AccountEmail == email && a.AccountId != exceptId);
        }

        public bool HasCreatedNews(short id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles.Any(n => n.CreatedById == id);
        }

        public void Add(SystemAccount account)
        {
            using var db = new FunewsManagementContext();
            var maxId = db.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0;
            account.AccountId = (short)(maxId + 1);
            account.NewsArticles = new List<NewsArticle>();
            db.SystemAccounts.Add(account);
            db.SaveChanges();
        }

        public bool Update(SystemAccount account)
        {
            using var db = new FunewsManagementContext();
            var existing = db.SystemAccounts.FirstOrDefault(a => a.AccountId == account.AccountId);
            if (existing == null) return false;

            existing.AccountName = account.AccountName;
            existing.AccountEmail = account.AccountEmail;
            existing.AccountRole = account.AccountRole;
            if (!string.IsNullOrEmpty(account.AccountPassword))
            {
                existing.AccountPassword = account.AccountPassword;
            }
            db.SaveChanges();
            return true;
        }

        public bool Delete(short id)
        {
            using var db = new FunewsManagementContext();
            var existing = db.SystemAccounts.FirstOrDefault(a => a.AccountId == id);
            if (existing == null) return false;
            db.SystemAccounts.Remove(existing);
            db.SaveChanges();
            return true;
        }
    }
}
