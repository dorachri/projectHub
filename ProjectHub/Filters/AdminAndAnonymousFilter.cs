using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectHub.Filters
{
    public class AdminAndAnonymousFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated && !context.HttpContext.User.IsInRole("Admin"))
            {
                context.Result = new RedirectToActionResult("Index", "Home", "");
            }
        }
    }
}
