using _30_TranTheTruong_Assignment01_BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace _30_TranTheTruong_Assignment01_BackEnd.DAOs
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO? _instance;
        private static readonly object _lock = new();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new NewsArticleDAO();
                }
            }
        }

        public List<NewsArticle> GetAll(bool onlyActive)
        {
            using var db = new FunewsManagementContext();
            var query = db.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .AsQueryable();

            if (onlyActive)
            {
                query = query.Where(n => n.NewsStatus == true);
            }

            return query.OrderByDescending(n => n.CreatedDate).ToList();
        }

        public NewsArticle? GetById(string id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles
                
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .FirstOrDefault(n => n.NewsArticleId == id);
        }

        public List<NewsArticle> GetByDateRange(DateTime start, DateTime endExclusive)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles
                
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= start && n.CreatedDate < endExclusive)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public string Add(NewsArticle article, IEnumerable<int> tagIds)
        {
            using var db = new FunewsManagementContext();
            article.NewsArticleId = NextId(db);
            article.Category = null;
            article.CreatedBy = null;
            article.Tags = db.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            db.NewsArticles.Add(article);
            db.SaveChanges();
            return article.NewsArticleId;
        }

        public bool Update(NewsArticle article, IEnumerable<int> tagIds)
        {
            using var db = new FunewsManagementContext();
            var existing = db.NewsArticles.Include(n => n.Tags)
                .FirstOrDefault(n => n.NewsArticleId == article.NewsArticleId);
            if (existing == null) return false;

            existing.NewsTitle = article.NewsTitle;
            existing.Headline = article.Headline;
            existing.NewsContent = article.NewsContent;
            existing.NewsSource = article.NewsSource;
            existing.CategoryId = article.CategoryId;
            existing.NewsStatus = article.NewsStatus;
            existing.UpdatedById = article.UpdatedById;
            existing.ModifiedDate = article.ModifiedDate;

            existing.Tags.Clear();
            foreach (var tag in db.Tags.Where(t => tagIds.Contains(t.TagId)).ToList())
            {
                existing.Tags.Add(tag);
            }
            db.SaveChanges();
            return true;
        }

        public bool Delete(string id)
        {
            using var db = new FunewsManagementContext();
            var existing = db.NewsArticles.Include(n => n.Tags).FirstOrDefault(n => n.NewsArticleId == id);
            if (existing == null) return false;
            existing.Tags.Clear();
            db.NewsArticles.Remove(existing);
            db.SaveChanges();
            return true;
        }

        // NewsArticleID is an nvarchar(20) that is not an identity column -> generate max(numeric id) + 1
        private static string NextId(FunewsManagementContext db)
        {
            var max = 0;
            foreach (var id in db.NewsArticles.Select(n => n.NewsArticleId).ToList())
            {
                if (int.TryParse(id, out var value) && value > max) max = value;
            }
            return (max + 1).ToString();
        }
    }
}
