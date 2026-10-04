using _30_TranTheTruong_Assignment01_FrontEnd.Filters;
using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    [RequireRole("Staff")]
    public class ProfileController : BaseController
    {
        private readonly ApiClient _api;

        public ProfileController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _api.GetProfileAsync();
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = result.Error ?? "Cannot load your profile.";
                return RedirectToAction("Index", "News");
            }
            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _api.UpdateProfileAsync(model);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Update failed.");
                return View(model);
            }

            HttpContext.Session.SetString("name", model.AccountName ?? string.Empty);
            HttpContext.Session.SetString("email", model.AccountEmail ?? string.Empty);
            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }

}
