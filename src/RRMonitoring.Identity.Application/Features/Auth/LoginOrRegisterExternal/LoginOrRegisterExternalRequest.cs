using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Enums;
using RRMonitoring.Identity.Application.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.ExternalProviders;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Application.Features.Auth.LoginOrRegisterExternal;

public record LoginOrRegisterExternalRequest(AuthenticateResult AuthResult) : IRequest<ExternalLoginResultDto>;

public class LoginOrRegisterExternalHandler : IRequestHandler<LoginOrRegisterExternalRequest, ExternalLoginResultDto>
{
	private readonly IExternalUserFactory _externalUserFactory;
	private readonly IdentityUserManager _userManager;
	private readonly IUserRepository _userRepository;
	private readonly IExternalSourceRepository _externalSourceRepository;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<LoginOrRegisterExternalHandler> _logger;

	public LoginOrRegisterExternalHandler(
		IExternalUserFactory externalUserFactory,
		IdentityUserManager userManager,
		IUserRepository userRepository,
		IExternalSourceRepository externalSourceRepository,
		IHttpContextAccessor httpContextAccessor,
		IDateTimeProvider dateTimeProvider,
		ILogger<LoginOrRegisterExternalHandler> logger)
	{
		_externalUserFactory = externalUserFactory;
		_userManager = userManager;
		_userRepository = userRepository;
		_externalSourceRepository = externalSourceRepository;
		_httpContextAccessor = httpContextAccessor;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
	}

	public async Task<ExternalLoginResultDto> Handle(LoginOrRegisterExternalRequest request, CancellationToken cancellationToken)
	{
		var providerName = request.AuthResult.Properties!.Items
			.FirstOrDefault(x => x.Key == "LoginProvider").Value;

		var claims = request.AuthResult.Principal!.Claims.ToList();

		var externalId = _externalUserFactory.GetUserExternalId(providerName, claims);
		if (externalId is null)
		{
			_logger.LogError("No ID claim from external provider, Claims: {Claims}",
				string.Join(",", claims.Select(x => $"{x.Type}: {x.Value}")));

			return ExternalLoginResultDto.Failed("No ID claim from external provider", ExternalAuthErrorCodes.Unauthorized);
		}

		var user = await _userRepository.GetByExternalId(externalId, cancellationToken: cancellationToken);
		if (user is null)
		{
			if (!_externalUserFactory.IsUserRegistrationEnabled(providerName))
			{
				return ExternalLoginResultDto.Failed(
					$"User with ExternalId: {externalId} doesn't exist",
					ExternalAuthErrorCodes.UserNotExist);
			}

			var externalUser = await _externalUserFactory.GetByProvider(providerName, externalId);
			var externalSource = await _externalSourceRepository.GetByCode(providerName, cancellationToken);

			if (externalSource is null)
			{
				throw new ValidationException($"External source for provider: {providerName} doesn't exist");
			}

			var identityResult = await _userManager.CreateAsync(user);
			if (!identityResult.Succeeded)
			{
				var errorMessages = string.Join(',', identityResult.Errors.Select(x => x.Description));

				_logger.LogError("Error when try to create user from external source: {ExternalSource}, messages: {Message}",
					externalSource.Id, errorMessages);

				throw new RegisterUserException($"Can't register user from external source: {externalSource.Id}");
			}
		}

		if (user.IsBlocked)
		{
			return ExternalLoginResultDto.Failed(
				$"User with ExternalId: {externalId} is blocked",
				ExternalAuthErrorCodes.UserBlocked);
		}

		if (user.StatusId == (byte)UserStatuses.OnApproval)
		{
			return ExternalLoginResultDto.Failed(
				$"User with ExternalId: {externalId} is not approved",
				ExternalAuthErrorCodes.UserNotApproved);
		}

		var identityUser = new IdentityServerUser(user.Id.ToString());
		await _httpContextAccessor.HttpContext.SignInAsync(identityUser);

		await _httpContextAccessor.HttpContext!.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

		user.LastLoginDate = _dateTimeProvider.GetUtcNow();
		await _userRepository.Update(user, cancellationToken);

		return ExternalLoginResultDto.Success();
	}
}
