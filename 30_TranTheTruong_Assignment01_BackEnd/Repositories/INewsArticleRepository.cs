using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetAll(bool onlyActive);
        NewsArticle? GetById(string id);
        List<NewsArticle> GetByDateRange(DateTime start, DateTime endExclusive);
        string Add(NewsArticle article, IEnumerable<int> tagIds);
        bool Update(NewsArticle article, IEnumerable<int> tagIds);
        bool Delete(string id);
    }
}
