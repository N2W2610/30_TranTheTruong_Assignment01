using _30_TranTheTruong_Assignment01_FrontEnd.Filters;
using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    [RequireRole("Staff")]
    public class CategoriesController : BaseController
    {
        private readonly ApiClient _api;

        public CategoriesController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var result = await _api.GetCategoriesAsync(search);
            if (!result.Success) ViewBag.Error = result.Error;

            // full list (not filtered) for the "parent category" drop-downs
            var all = await _api.GetCategoriesAsync(null);
            ViewBag.AllCategories = all.Data ?? new List<CategoryVM>();
            ViewBag.Search = search;
            return View(result.Data ?? new List<CategoryVM>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM model)
        {
            if (!ModelState.IsValid) return Invalid();

            return Done(await _api.CreateCategoryAsync(model), "Category created successfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM model)
        {
            if (!ModelState.IsValid) return Invalid();

            return Done(await _api.UpdateCategoryAsync(model), "Category updated successfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            return Done(await _api.DeleteCategoryAsync(id), "Category deleted successfully.");
        }
    }
}
