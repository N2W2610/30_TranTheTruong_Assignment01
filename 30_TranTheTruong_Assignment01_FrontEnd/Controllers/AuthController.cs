using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ApiClient _api;

        public AuthController(ApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login(bool expired = false)
        {
            if (expired) ViewBag.Error = "Your session has expired. Please sign in again.";
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _api.LoginAsync(model);
            if (!result.Success || result.Data == null)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Login failed.");
                return View(model);
            }

            var user = result.Data;
            HttpContext.Session.SetString("token", user.Token);
            HttpContext.Session.SetString("role", user.Role);
            HttpContext.Session.SetString("name", user.Name);
            HttpContext.Session.SetString("email", user.Email);
            HttpContext.Session.SetString("id", user.AccountId.ToString());

            return user.Role == "Admin"
                ? RedirectToAction("Index", "Accounts")
                : RedirectToAction("Index", "News");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }

}
