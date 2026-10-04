using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApiClient _api;

        public HomeController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var result = await _api.GetNewsAsync(search, null, onlyActive: true);
            if (!result.Success) ViewBag.Error = result.Error;

            ViewBag.Search = search;
            return View(result.Data ?? new List<NewsVM>());
        }

        public async Task<IActionResult> Details(string id)
        {
            var result = await _api.GetNewsByIdAsync(id);
            if (!result.Success || result.Data == null || result.Data.NewsStatus != true)
            {
                TempData["Error"] = "The news article was not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(result.Data);
        }

        public IActionResult Error() => View();
    }

}
