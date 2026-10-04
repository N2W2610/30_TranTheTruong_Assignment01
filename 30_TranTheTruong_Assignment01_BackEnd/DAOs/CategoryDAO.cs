using _30_TranTheTruong_Assignment01_BackEnd.Models;

namespace _30_TranTheTruong_Assignment01_BackEnd.DAOs
{
    public class CategoryDAO
    {
        private static CategoryDAO? _instance;
        private static readonly object _lock = new();

        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new CategoryDAO();
                }
            }
        }

        public List<Category> GetAll()
        {
            using var db = new FunewsManagementContext();
            return db.Categories.OrderBy(c => c.CategoryId).ToList();
        }

        public Category? GetById(short id)
        {
            using var db = new FunewsManagementContext();
            return db.Categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public bool IsUsedByNews(short id)
        {
            using var db = new FunewsManagementContext();
            return db.NewsArticles.Any(n => n.CategoryId == id);
        }

        public bool HasChildren(short id)
        {
            using var db = new FunewsManagementContext();
            return db.Categories.Any(c => c.ParentCategoryId == id && c.CategoryId != id);
        }

        public void Add(Category category)
        {
            using var db = new FunewsManagementContext();
            category.CategoryId = 0;
            category.ParentCategory = null;
            category.InverseParentCategory = new List<Category>();
            category.NewsArticles = new List<NewsArticle>();
            db.Categories.Add(category);
            db.SaveChanges();
        }

        public bool Update(Category category)
        {
            using var db = new FunewsManagementContext();
            var existing = db.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
            if (existing == null) return false;

            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            existing.ParentCategoryId = category.ParentCategoryId;
            existing.IsActive = category.IsActive;
            db.SaveChanges();
            return true;
        }

        public bool Delete(short id)
        {
            using var db = new FunewsManagementContext();
            var existing = db.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (existing == null) return false;
            db.Categories.Remove(existing);
            db.SaveChanges();
            return true;
        }
    }

}
