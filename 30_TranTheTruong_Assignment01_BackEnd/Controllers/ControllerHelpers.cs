using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;
using System.Text.Json;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    public static class ClaimsExtensions
    {
        public static short GetAccountId(this ClaimsPrincipal user) =>
            short.TryParse(user.FindFirst("sub")?.Value, out var id) ? id : (short)0;
    }

    /// <summary>
    /// Base class for OData controllers. Errors are always written as plain JSON { "error": "..." }
    /// so the client can read them the same way for OData and non-OData endpoints.
    /// </summary>
    public abstract class ODataApiController : ODataController
    {
        protected IActionResult Fail(string message, int status = StatusCodes.Status400BadRequest) =>
            new ContentResult
            {
                Content = JsonSerializer.Serialize(new { error = message }),
                ContentType = "application/json",
                StatusCode = status
            };

        protected IActionResult InvalidModel()
        {
            var messages = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m));
            return Fail(string.Join(" ", messages));
        }

        protected IActionResult FromResult(ServiceResult result, Func<IActionResult> onSuccess)
        {
            if (result.NotFound) return NotFound();
            if (!result.Success) return Fail(result.Error ?? "Request failed.");
            return onSuccess();
        }
    }
}
