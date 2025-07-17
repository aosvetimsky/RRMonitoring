using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using RRMonitoring.Identity.Application.Enums;

namespace RRMonitoring.Identity.Application.Services.Link;

public class LinkService : ILinkService
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly LinkGenerator _linkGenerator;
	private readonly string _pathBase;

	private const string ResetPasswordControllerName = "ResetPassword";
	private const string ResetPasswordActionName = "ResetPassword";

	private const string UserConfirmEmailAction = "ConfirmRegistration";
	private const string UserConfirmEmailControllerName = "Registration";

	public LinkService(
		IHttpContextAccessor httpContextAccessor,
		LinkGenerator linkGenerator,
		IConfiguration configuration)
	{
		_httpContextAccessor = httpContextAccessor;
		_linkGenerator = linkGenerator;

		_pathBase = configuration.GetValue<string>("PathBase");
	}

	public string GenerateResetPasswordLink(
		Guid userId,
		string token,
		ResetPasswordErrorCodes? errorCode = null)
	{
		var httpContext = _httpContextAccessor.HttpContext;

		return _linkGenerator.GetUriByAction(
			httpContext,
			action: ResetPasswordActionName,
			controller: ResetPasswordControllerName,
			pathBase: _pathBase,
			values: new { userId, token, errorCode },
			scheme: httpContext.Request.Scheme);
	}

	public string GenerateConfirmEmailLink(
		Guid userId,
		string token)
	{
		var httpContext = _httpContextAccessor.HttpContext;

		return _linkGenerator.GetUriByAction(
			httpContext,
			action: UserConfirmEmailAction,
			controller: UserConfirmEmailControllerName,
			pathBase: _pathBase,
			values: new { userId, token },
			scheme: httpContext.Request.Scheme);
	}
}
