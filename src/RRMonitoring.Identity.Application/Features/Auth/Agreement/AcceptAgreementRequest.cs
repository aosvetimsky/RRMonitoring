using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using RRMonitoring.Identity.Application.Features.Auth.Login;
using RRMonitoring.Identity.Application.Services.Agreement;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Auth.Agreement;

public class AcceptAgreementRequest : IRequest<LoginResultDto>
{
}

public class AcceptAgreementHandler : IRequestHandler<AcceptAgreementRequest, LoginResultDto>
{
	private const string UserValidationError = "Ошибка проверки пользователя.";

	private readonly IVerifiedLoginService _verifiedLoginService;
	private readonly IdentityUserManager _userManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ILogger<AcceptAgreementHandler> _logger;

	public AcceptAgreementHandler(
		IVerifiedLoginService cookieService,
		IdentityUserManager userManager,
		IDateTimeProvider dateTimeProvider,
		IHttpContextAccessor httpContextAccessor,
		ILogger<AcceptAgreementHandler> logger)
	{
		_verifiedLoginService = cookieService;
		_userManager = userManager;
		_dateTimeProvider = dateTimeProvider;
		_httpContextAccessor = httpContextAccessor;
		_logger = logger;
	}

	public async Task<LoginResultDto> Handle(AcceptAgreementRequest request, CancellationToken cancellationToken)
	{
		var verifiedUserId = await _verifiedLoginService.GetVerifiedUserIdAsync();
		if (verifiedUserId == null)
		{
			_logger.LogInformation("Verified login cookies have expired or are not set or invalid");

			return LoginResultDto.Failed(UserValidationError);
		}

		var user = await _userManager.FindByIdAsync(verifiedUserId);
		if (user == null)
		{
			_logger.LogInformation("Сan\'t find user with id: {UserId}", verifiedUserId);

			return LoginResultDto.Failed(UserValidationError);
		}

		await SetAgreementAcceptedDateAsync(user);

		await SignInAsync(user);

		_verifiedLoginService.ForgetVerifiedLogin();

		return LoginResultDto.Success();
	}

	private async Task SetAgreementAcceptedDateAsync(User user)
	{
		user.AgreementAcceptedDate = _dateTimeProvider.GetUtcNow();
		await _userManager.UpdateAsync(user);
	}

	private async Task SignInAsync(User user)
	{
		var identityServerUser = new IdentityServerUser(user.Id.ToString()) { DisplayName = user.UserName, };

		await _httpContextAccessor.HttpContext.SignInAsync(identityServerUser);
	}
}
