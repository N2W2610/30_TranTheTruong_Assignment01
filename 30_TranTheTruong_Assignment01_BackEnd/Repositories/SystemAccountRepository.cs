using _30_TranTheTruong_Assignment01_BackEnd.DAOs;
using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public List<SystemAccount> GetAll() => SystemAccountDAO.Instance.GetAll();
        public SystemAccount? GetById(short id) => SystemAccountDAO.Instance.GetById(id);
        public SystemAccount? Login(string email, string password) => SystemAccountDAO.Instance.Login(email, password);
        public bool EmailExists(string email, short exceptId) => SystemAccountDAO.Instance.EmailExists(email, exceptId);
        public bool HasCreatedNews(short id) => SystemAccountDAO.Instance.HasCreatedNews(id);
        public void Add(SystemAccount account) => SystemAccountDAO.Instance.Add(account);
        public bool Update(SystemAccount account) => SystemAccountDAO.Instance.Update(account);
        public bool Delete(short id) => SystemAccountDAO.Instance.Delete(id);
    }

}
