using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ApiUnauthorizedException)
            {
                context.HttpContext.Session.Clear();
                context.Result = new RedirectToActionResult("Login", "Auth", new { expired = true });
                context.ExceptionHandled = true;
            }
        }
    }
}
