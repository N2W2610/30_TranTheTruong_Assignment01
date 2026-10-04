using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface INewsService
    {
        List<NewsArticle> GetArticles(bool onlyActive);
        ServiceResult Create(NewsArticleRequest request, short userId);
        ServiceResult Update(string id, NewsArticleRequest request, short userId);
        ServiceResult Delete(string id);
    }

    public class NewsService : INewsService
    {
        private readonly INewsArticleRepository _news;
        private readonly ICategoryRepository _categories;

        public NewsService(INewsArticleRepository news, ICategoryRepository categories)
        {
            _news = news;
            _categories = categories;
        }

        public List<NewsArticle> GetArticles(bool onlyActive)
        {
            var articles = _news.GetAll(onlyActive);
            foreach (var article in articles)
            {
                // Only expose the creator's id and name (never email/password).
                if (article.CreatedBy != null)
                {
                    article.CreatedBy = new SystemAccount
                    {
                        AccountId = article.CreatedBy.AccountId,
                        AccountName = article.CreatedBy.AccountName
                    };
                }
            }
            return articles;
        }

        public ServiceResult Create(NewsArticleRequest request, short userId)
        {
            var check = ValidateCategory(request.CategoryID!.Value);
            if (!check.Success) return check;

            var now = DateTime.Now;
            var article = new NewsArticle
            {
                NewsTitle = request.NewsTitle!.Trim(),
                Headline = request.Headline!.Trim(),
                NewsContent = request.NewsContent!.Trim(),
                NewsSource = request.NewsSource?.Trim(),
                CategoryId = request.CategoryID,
                NewsStatus = request.NewsStatus,
                CreatedById = userId,
                UpdatedById = userId,
                CreatedDate = now,
                ModifiedDate = now
            };

            _news.Add(article, request.TagIds.Distinct());
            return ServiceResult.Ok();
        }

        public ServiceResult Update(string id, NewsArticleRequest request, short userId)
        {
            if (_news.GetById(id) == null) return ServiceResult.Missing("News article not found.");

            var check = ValidateCategory(request.CategoryID!.Value);
            if (!check.Success) return check;

            var article = new NewsArticle
            {
                NewsArticleId = id,
                NewsTitle = request.NewsTitle!.Trim(),
                Headline = request.Headline!.Trim(),
                NewsContent = request.NewsContent!.Trim(),
                NewsSource = request.NewsSource?.Trim(),
                CategoryId = request.CategoryID,
                NewsStatus = request.NewsStatus,
                UpdatedById = userId,
                ModifiedDate = DateTime.Now
            };

            return _news.Update(article, request.TagIds.Distinct())
                ? ServiceResult.Ok()
                : ServiceResult.Missing("News article not found.");
        }

        public ServiceResult Delete(string id)
        {
            return _news.Delete(id) ? ServiceResult.Ok() : ServiceResult.Missing("News article not found.");
        }

        private ServiceResult ValidateCategory(short categoryId)
        {
            var category = _categories.GetById(categoryId);
            if (category == null) return ServiceResult.Fail("The selected category does not exist.");
            if (category.IsActive != true) return ServiceResult.Fail("The selected category is inactive.");
            return ServiceResult.Ok();
        }
    }
}
