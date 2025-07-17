using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Auth.Logout;

public class LogoutRequest : IRequest<string>
{
	public LogoutRequest(string logoutId)
	{
		LogoutId = logoutId;
	}

	public string LogoutId { get; set; }
}

public class LogoutHandler : IRequestHandler<LogoutRequest, string>
{
	private readonly IIdentityServerInteractionService _identityInteractionService;
	private readonly SignInManager<User> _signInManager;
	private readonly ILogger<LogoutHandler> _logger;

	public LogoutHandler(
		IIdentityServerInteractionService identityInteractionService,
		SignInManager<User> signInManager,
		ILogger<LogoutHandler> logger)
	{
		_identityInteractionService = identityInteractionService;
		_signInManager = signInManager;
		_logger = logger;
	}

	public async Task<string> Handle(LogoutRequest request, CancellationToken cancellationToken)
	{
		var logoutContext = await _identityInteractionService
			.GetLogoutContextAsync(request.LogoutId);

		if (logoutContext == null)
		{
			_logger.LogError("Wrong logout Id");

			throw new ValidationException("Ошибка при выходе из аккаунта.");
		}

		await _signInManager.SignOutAsync();

		return logoutContext.PostLogoutRedirectUri;
	}
}
