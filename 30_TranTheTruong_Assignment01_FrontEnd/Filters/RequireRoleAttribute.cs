using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Filters
{
    public class RequireRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _roles;

        public RequireRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var role = context.HttpContext.Session.GetString("role");

            if (string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (_roles.Length > 0 && !_roles.Contains(role))
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            }
        }
    }

}
