using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Repositories;

namespace _30_TranTheTruong_Assignment01_BackEnd.Services
{
    public interface IReportService
    {
        ReportResponse Build(DateTime startDate, DateTime endDate);
    }

    public class ReportService : IReportService
    {
        private readonly INewsArticleRepository _news;

        public ReportService(INewsArticleRepository news)
        {
            _news = news;
        }

        public ReportResponse Build(DateTime startDate, DateTime endDate)
        {
            var start = startDate.Date;
            var endExclusive = endDate.Date.AddDays(1);

            // Repository returns data already sorted by CreatedDate descending.
            var articles = _news.GetByDateRange(start, endExclusive);

            return new ReportResponse
            {
                StartDate = start,
                EndDate = endDate.Date,
                Total = articles.Count,
                ActiveCount = articles.Count(a => a.NewsStatus == true),
                InactiveCount = articles.Count(a => a.NewsStatus != true),
                ByCategory = articles
                    .GroupBy(a => a.Category?.CategoryName ?? "(none)")
                    .Select(g => new ReportGroup { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList(),
                ByCreator = articles
                    .GroupBy(a => a.CreatedBy?.AccountName ?? "(unknown)")
                    .Select(g => new ReportGroup { Name = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList(),
                Articles = articles.Select(a => new ReportItem
                {
                    NewsArticleId = a.NewsArticleId,
                    NewsTitle = a.NewsTitle,
                    Headline = a.Headline,
                    CreatedDate = a.CreatedDate,
                    CategoryName = a.Category?.CategoryName,
                    CreatedByName = a.CreatedBy?.AccountName,
                    NewsStatus = a.NewsStatus == true
                }).ToList()
            };
        }
    }

}
