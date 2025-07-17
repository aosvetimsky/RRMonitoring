using System;
using Microsoft.AspNetCore.Mvc;

namespace RRMonitoring.Identity.Api.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class InternalRouteAttribute : RouteAttribute
{
	public InternalRouteAttribute(string template) : base("internal/" + template)
	{
	}
}
