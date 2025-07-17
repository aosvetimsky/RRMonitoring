using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.UserValidation;

public class UserAccessVerificationService : IUserAccessVerificationService
{
	private readonly ICurrentUserService<CurrentUserBase> _currentUserService;
	private readonly IdentityUserManager _userManager;

	public UserAccessVerificationService(
		ICurrentUserService<CurrentUserBase> currentUserService,
		IdentityUserManager userManager)
	{
		_currentUserService = currentUserService;
		_userManager = userManager;
	}

	public async Task<User> VerifyCodeAndRetrieveUser(string verificationCode, CancellationToken cancellationToken)
	{
		var currentUserId = _currentUserService.CurrentUserId;
		var user = await _userManager.FindByIdAsync(currentUserId.ToString());

		if (user is null)
		{
			throw new ValidationException("User does not exist.");
		}

		var isValidTwoFactorCode = await _userManager.ValidateTwoFactorCode(user, verificationCode);

		if (!isValidTwoFactorCode)
		{
			throw new ValidationException("Verification code is invalid");
		}

		return user;
	}
}
