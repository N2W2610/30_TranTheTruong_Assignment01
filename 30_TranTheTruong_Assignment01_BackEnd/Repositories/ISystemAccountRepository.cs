using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetAll();
        SystemAccount? GetById(short id);
        SystemAccount? Login(string email, string password);
        bool EmailExists(string email, short exceptId);
        bool HasCreatedNews(short id);
        void Add(SystemAccount account);
        bool Update(SystemAccount account);
        bool Delete(short id);
    }
}
