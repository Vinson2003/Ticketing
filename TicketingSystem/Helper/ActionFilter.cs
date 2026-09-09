using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketingSystem.Helper;

public class ActionFilter : IAsyncActionFilter
{
    private static bool IsAjaxRequest(HttpRequest request)
    {
        return request.Headers.XRequestedWith == "XMLHttpRequest";
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            await next();
            return;
        }

        var descriptor = context.ActionDescriptor as ControllerActionDescriptor;

        if (descriptor == null)
        {
            await next();
            return;
        }

        var controllerName = descriptor.ControllerName.ToLower();

        var actionName = descriptor.ActionName.ToLower();

        var requiredPermission = $"{controllerName}-{actionName}";

        var permissions = Cache.CachePerms(context.HttpContext);

        if (context.Controller is Controller controller)
        {
            controller.ViewBag.Perms = permissions;
        }

        var excludedControllers = new List<string>
        {
            "account",
            "home",
            "permission"
        };

        var excludedActions = new List<string>
        {
            "ticket-gettickets",
            "ticket-gettickethistory"
        };

        if (excludedControllers.Contains(controllerName) || excludedActions.Contains(requiredPermission))
        {
            await next();
            return;
        }

        var hasPermission = PermissionHelper.CheckPerms(user, permissions, requiredPermission);
        if (hasPermission)
        {
            await next();
            return;
        }

        if (IsAjaxRequest(context.HttpContext.Request))
        {
            context.Result = new JsonResult(new
            {
                Success = false,
                Message = "You do not have permission to perform this action."
            });

            return;
        }

        context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
    }
}