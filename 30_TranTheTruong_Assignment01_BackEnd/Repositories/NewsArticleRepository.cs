using _30_TranTheTruong_Assignment01_BackEnd.DAOs;
using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public List<NewsArticle> GetAll(bool onlyActive) => NewsArticleDAO.Instance.GetAll(onlyActive);
        public NewsArticle? GetById(string id) => NewsArticleDAO.Instance.GetById(id);
        public List<NewsArticle> GetByDateRange(DateTime start, DateTime endExclusive) => NewsArticleDAO.Instance.GetByDateRange(start, endExclusive);
        public string Add(NewsArticle article, IEnumerable<int> tagIds) => NewsArticleDAO.Instance.Add(article, tagIds);
        public bool Update(NewsArticle article, IEnumerable<int> tagIds) => NewsArticleDAO.Instance.Update(article, tagIds);
        public bool Delete(string id) => NewsArticleDAO.Instance.Delete(id);
    }

}
