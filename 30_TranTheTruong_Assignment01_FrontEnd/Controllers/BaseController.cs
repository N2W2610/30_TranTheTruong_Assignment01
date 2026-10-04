using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string ModelErrors() =>
            string.Join(" ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m)));

        protected short CurrentAccountId =>
            short.TryParse(HttpContext.Session.GetString("id"), out var id) ? id : (short)0;

        /// <summary>Stores a success / error message (shown as an alert) and redirects.</summary>
        protected IActionResult Done(ApiResult result, string successMessage, string action = "Index")
        {
            if (result.Success) TempData["Success"] = successMessage;
            else TempData["Error"] = result.Error;
            return RedirectToAction(action);
        }

        protected IActionResult Invalid(string action = "Index")
        {
            TempData["Error"] = ModelErrors();
            return RedirectToAction(action);
        }
    }

}
