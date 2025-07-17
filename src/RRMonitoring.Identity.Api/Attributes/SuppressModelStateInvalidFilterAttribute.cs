using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace RRMonitoring.Identity.Api.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class SuppressModelStateInvalidFilterAttribute : Attribute, IActionModelConvention
{
	private const string FilterTypeName = "ModelStateInvalidFilterFactory";

	public void Apply(ActionModel action)
	{
		var removingFilter = action.Filters.FirstOrDefault(x => x.GetType().Name == FilterTypeName);

		if (removingFilter != null)
		{
			action.Filters.Remove(removingFilter);
		}
	}
}
