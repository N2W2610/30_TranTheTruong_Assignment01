using _30_TranTheTruong_Assignment01_FrontEnd.Filters;
using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    [RequireRole("Staff")]
    public class NewsController : BaseController
    {
        private readonly ApiClient _api;

        public NewsController(ApiClient api)
        {
            _api = api;
        }

        // All news articles (managed by staff)
        public Task<IActionResult> Index(string? search) => BuildPage(search, historyOnly: false);

        // News articles created by the current staff member
        public Task<IActionResult> History(string? search) => BuildPage(search, historyOnly: true);

        private async Task<IActionResult> BuildPage(string? search, bool historyOnly)
        {
            var news = await _api.GetNewsAsync(search, historyOnly ? CurrentAccountId : null, onlyActive: false);
            var categories = await _api.GetCategoriesAsync(null);
            var tags = await _api.GetTagsAsync();

            if (!news.Success) ViewBag.Error = news.Error;

            var page = new NewsPageVM
            {
                Items = news.Data ?? new List<NewsVM>(),
                Categories = categories.Data ?? new List<CategoryVM>(),
                Tags = tags.Data ?? new List<TagVM>(),
                Search = search,
                HistoryOnly = historyOnly
            };
            return View("Index", page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsFormVM model)
        {
            var back = BackAction(model.ReturnTo);
            if (!ModelState.IsValid) return Invalid(back);

            return Done(await _api.CreateNewsAsync(model), "News article created successfully.", back);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsFormVM model)
        {
            var back = BackAction(model.ReturnTo);
            if (string.IsNullOrWhiteSpace(model.NewsArticleId))
                ModelState.AddModelError(nameof(model.NewsArticleId), "News article id is missing.");
            if (!ModelState.IsValid) return Invalid(back);

            return Done(await _api.UpdateNewsAsync(model), "News article updated successfully.", back);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, string? returnTo)
        {
            return Done(await _api.DeleteNewsAsync(id), "News article deleted successfully.", BackAction(returnTo));
        }

        private static string BackAction(string? returnTo) => returnTo == nameof(History) ? nameof(History) : nameof(Index);
    }

}
