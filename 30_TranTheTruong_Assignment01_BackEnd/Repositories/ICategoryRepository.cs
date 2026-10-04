using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category? GetById(short id);
        bool IsUsedByNews(short id);
        bool HasChildren(short id);
        void Add(Category category);
        bool Update(Category category);
        bool Delete(short id);
    }
}
