using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EImzaTakip.Filters
{
    public class SessionAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(
            AuthorizationFilterContext context)
        {
            var userId =
                context.HttpContext.Session
                .GetInt32("UserId");

            // GİRİŞ YOKSA
            if (userId == null)
            {
                context.Result =
                    new RedirectToActionResult(
                        "Login",
                        "User",
                        null);
            }
        }
    }
}
