using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.DAOs;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetAll() => CategoryDAO.Instance.GetAll();
        public Category? GetById(short id) => CategoryDAO.Instance.GetById(id);
        public bool IsUsedByNews(short id) => CategoryDAO.Instance.IsUsedByNews(id);
        public bool HasChildren(short id) => CategoryDAO.Instance.HasChildren(id);
        public void Add(Category category) => CategoryDAO.Instance.Add(category);
        public bool Update(Category category) => CategoryDAO.Instance.Update(category);
        public bool Delete(short id) => CategoryDAO.Instance.Delete(id);
    }

}
