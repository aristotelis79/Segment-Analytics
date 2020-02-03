using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;

namespace Nop.Plugin.Widgets.SegmentAnalytics.ActionsFilters
{
    public class IdentityActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
           //Not need anything
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var controller = ((Controller) context.Controller);

            if (!CheckControllerAndAction(context.HttpContext.Request, controller.ControllerContext.ActionDescriptor,"Customer", "Login"))
                return;

            if (!context.ModelState.IsValid || context.ModelState.ErrorCount != 0) 
                return;

            if(!controller.TempData.ContainsKey("identify"))
                controller.TempData.Add("identify",1);
        }

        private static bool CheckControllerAndAction(HttpRequest httpRequest, ControllerActionDescriptor controllerActionDescriptor, string controllerName, string actionName)
        {
           return httpRequest.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase)
                  && controllerActionDescriptor.ControllerName.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase)
                  && controllerActionDescriptor.ActionName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}