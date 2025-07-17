using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RRMonitoring.Identity.Api.Filters;

public class MvcGlobalExceptionsFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor
			&& typeof(Controller).IsAssignableFrom(controllerActionDescriptor.ControllerTypeInfo.AsType()))
		{
			context.Result = new ViewResult { ViewName = "~/Views/Error.cshtml" };

			context.ExceptionHandled = true;
		}
	}
}
