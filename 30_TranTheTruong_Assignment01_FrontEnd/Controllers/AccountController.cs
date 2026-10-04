using _30_TranTheTruong_Assignment01_FrontEnd.Filters;
using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{

    [RequireRole("Admin")]
    public class AccountsController : BaseController
    {
        private readonly ApiClient _api;

        public AccountsController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(string? search)
        {
            var result = await _api.GetAccountsAsync(search);
            if (!result.Success) ViewBag.Error = result.Error;

            ViewBag.Search = search;
            return View(result.Data ?? new List<AccountVM>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountVM model)
        {
            if (string.IsNullOrWhiteSpace(model.AccountPassword))
                ModelState.AddModelError(nameof(model.AccountPassword), "Password is required.");
            if (!ModelState.IsValid) return Invalid();

            return Done(await _api.CreateAccountAsync(model), "Account created successfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountVM model)
        {
            if (!ModelState.IsValid) return Invalid();

            return Done(await _api.UpdateAccountAsync(model), "Account updated successfully.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            return Done(await _api.DeleteAccountAsync(id), "Account deleted successfully.");
        }
    }

}
