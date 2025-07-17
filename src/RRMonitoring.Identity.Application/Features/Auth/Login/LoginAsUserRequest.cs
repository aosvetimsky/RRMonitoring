using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.Auth.Login;

public class LoginAsUserRequest : IRequest<LoginResultDto>
{
	[Required]
	public Guid UserId { get; set; }

	public LoginAsUserRequest(Guid userId)
	{
		UserId = userId;
	}
}

public class LoginAsUserHandler : IRequestHandler<LoginAsUserRequest, LoginResultDto>
{
	private readonly IdentityUserManager _userManager;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ILogger<LoginAsUserHandler> _logger;

	public LoginAsUserHandler(
		IdentityUserManager userManager,
		IHttpContextAccessor httpContextAccessor,
		ILogger<LoginAsUserHandler> logger)
	{
		_userManager = userManager;
		_httpContextAccessor = httpContextAccessor;
		_logger = logger;
	}

	public async Task<LoginResultDto> Handle(LoginAsUserRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());

		if (user == null)
		{
			_logger.LogInformation("Сan\'t find user with id: {UserId}", request.UserId.ToString());

			return LoginResultDto.Failed("Пользователь не найден");
		}

		if (user.IsBlocked)
		{
			_logger.LogInformation("User with id: {UserId} is blocked", request.UserId.ToString());

			return LoginResultDto.Failed("Пользователь заблокирован");
		}

		var identityServerUser = new IdentityServerUser(user.Id.ToString())
		{
			DisplayName = user.UserName
		};

		await _httpContextAccessor.HttpContext.SignInAsync(identityServerUser);

		return LoginResultDto.Success();
	}
}
