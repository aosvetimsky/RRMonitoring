using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Features.GoogleAuth.Notification;
using RRMonitoring.Identity.Application.Services.UserManager.Notification;
using RRMonitoring.Identity.Domain.Contracts;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Services.UserManager;

public class IdentityUserManager : UserManager<User>
{
	public const string TwoFactorPurpose = "TwoFactor";
	public const string ResetAuthenticatorPurpose = "ResetAuthenticator";

	public new IdentityOptions Options { get; set; }

	private const string SamePasswordError = "Данный пароль уже использовался пользователем";
	private const string TooOftenPasswordChangeError = "Повторная смена пароля доступна через {0} ч.";

	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly ILogger<IdentityUserManager> _logger;
	private readonly AuthenticationConfig _authenticationConfig;

	private readonly int _functionsBlockTimeInHours;

	private const string AuthenticatorLoginProvider = "[AspNetUserStore]";
	private const string AuthenticatorTokenName = "AuthenticatorKey";

	public IdentityUserManager(
		IUserStore<User> store,
		IRabbitNotificationManager notificationManager,
		IOptions<RedRockIdentityOptions> options,
		IOptions<IdentityOptions> defaultOptions,
		IOptions<AuthenticationConfig> authConfig,
		IPasswordHasher<User> passwordHasher,
		IEnumerable<IUserValidator<User>> userValidators,
		IEnumerable<IPasswordValidator<User>> passwordValidators,
		ILookupNormalizer keyNormalizer,
		IdentityErrorDescriber errors,
		IServiceProvider services,
		IDateTimeProvider dateTimeProvider,
		IOptions<TimeoutConfig> timeoutOptions,
		ILogger<IdentityUserManager> logger)
		: base(store, defaultOptions, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors,
			services, logger)
	{
		_notificationManager = notificationManager;
		_dateTimeProvider = dateTimeProvider;
		_logger = logger;
		_authenticationConfig = authConfig.Value;

		Options = options?.Value ?? new RedRockIdentityOptions();

		_functionsBlockTimeInHours = timeoutOptions.Value.FunctionsBlockTimeInHours;
	}

	#region Overrided methods

	public override bool SupportsUserTwoFactor
	{
		get
		{
			ThrowIfDisposed();

			return _authenticationConfig.IsTwoFactorAuthenticationEnabled && Store is IUserTwoFactorStore<User>;
		}
	}

	public override async Task<IdentityResult> ChangePasswordAsync(
		User user, string currentPassword,
		string newPassword)
	{
		if (await IsPreviousPassword(user, newPassword))
		{
			return IdentityResult.Failed(new IdentityError { Description = SamePasswordError });
		}

		if (!await CanChangePassword(user))
		{
			var options = (RedRockIdentityOptions)Options;
			var minChangePasswordHours = options.Password.MinHoursBetweenPasswordChange;

			return IdentityResult.Failed(new IdentityError
			{
				Description = string.Format(TooOftenPasswordChangeError, minChangePasswordHours)
			});
		}

		var result = await base.ChangePasswordAsync(user, currentPassword, newPassword);
		if (result.Succeeded)
		{
			var applicationStore = (IApplicationUserStore)Store;
			await applicationStore.AddUsedPassword(user.Id, PasswordHasher.HashPassword(user, newPassword));

			var blockEndDate = _dateTimeProvider.GetUtcNow().AddHours(_functionsBlockTimeInHours);
			await applicationStore.AddEvent(user.Id, UserEventKinds.PasswordChanged, blockEndDate);
		}

		return result;
	}

	public async Task<IdentityResult> RemoveAuthenticator(User user)
	{
		if (user is null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		var result = await RemoveAuthenticationTokenAsync(user, AuthenticatorLoginProvider, AuthenticatorTokenName);
		if (result.Succeeded)
		{
			var applicationStore = (IApplicationUserStore)Store;

			var blockEndDate = _dateTimeProvider.GetUtcNow().AddHours(_functionsBlockTimeInHours);
			await applicationStore.AddEvent(user.Id, UserEventKinds.AuthenticatorDisabled, blockEndDate);

			if (user.TwoFactorEnabled)
			{
				result = await SetTwoFactorEnabledAsync(user, false);
			}
		}

		return result;
	}

	public override async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
	{
		if (await IsPreviousPassword(user, newPassword))
		{
			return IdentityResult.Failed(new IdentityError { Description = SamePasswordError });
		}

		if (!await CanChangePassword(user))
		{
			var options = (RedRockIdentityOptions)Options;
			var minChangePasswordHours = options.Password.MinHoursBetweenPasswordChange;

			return IdentityResult.Failed(new IdentityError
			{
				Description = string.Format(TooOftenPasswordChangeError, minChangePasswordHours)
			});
		}

		var result = await base.ResetPasswordAsync(user, token, newPassword);
		if (result.Succeeded)
		{
			var applicationStore = (IApplicationUserStore)Store;
			await applicationStore.AddUsedPassword(user.Id, PasswordHasher.HashPassword(user, newPassword));

			var blockEndDate = _dateTimeProvider.GetUtcNow().AddHours(_functionsBlockTimeInHours);
			await applicationStore.AddEvent(user.Id, UserEventKinds.ResetPassword, blockEndDate);
		}

		return result;
	}

	public override async Task<IdentityResult> ChangePhoneNumberAsync(User user, string phoneNumber, string token)
	{
		var result = await base.ChangePhoneNumberAsync(user, phoneNumber, token);
		if (result.Succeeded)
		{
			var applicationStore = (IApplicationUserStore)Store;

			var blockEndDate = _dateTimeProvider.GetUtcNow().AddHours(_functionsBlockTimeInHours);
			await applicationStore.AddEvent(user.Id, UserEventKinds.PhoneNumberChanged, blockEndDate);
		}

		return result;
	}

	public override async Task<IdentityResult> ChangeEmailAsync(User user, string newEmail, string token)
	{
		var result = await base.ChangeEmailAsync(user, newEmail, token);
		if (result.Succeeded)
		{
			var applicationStore = (IApplicationUserStore)Store;

			var blockEndDate = _dateTimeProvider.GetUtcNow().AddHours(_functionsBlockTimeInHours);
			await applicationStore.AddEvent(user.Id, UserEventKinds.EmailChanged, blockEndDate);
		}

		return result;
	}

	public override async Task<IdentityResult> AccessFailedAsync(User user)
	{
		ThrowIfDisposed();
		var store = (IUserLockoutStore<User>)Store;
		if (user is null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		// If this puts the user over the threshold for lockout, lock them out and reset the access failed count
		var count = await store.IncrementAccessFailedCountAsync(user, CancellationToken);
		if (count < Options.Lockout.MaxFailedAccessAttempts)
		{
			return await UpdateUserAsync(user);
		}

		Logger.LogWarning(new EventId(12, "UserLockedOut"), "User is locked out.");

		var endDate = DateTimeOffset.UtcNow.Add(Options.Lockout.DefaultLockoutTimeSpan);
		await store.SetLockoutEndDateAsync(user, endDate, CancellationToken);
		await store.ResetAccessFailedCountAsync(user, CancellationToken);

		var notification = new UserLockoutNotification
		{
			Recipient = user.Email,
			RecipientId = user.Id,
			Username = user.UserName,
			EndDateLockout = $"{endDate.ToUniversalTime():yyyy-MM-dd HH:mm} (UTC)"
		};
		await _notificationManager.SendEmail(notification);

		return await UpdateUserAsync(user);
	}

	#endregion

	#region Get users by properties

	public virtual async Task<User> GetByLogin(string login)
	{
		var signInOptions = (RedRockIdentityOptions)Options;

		if (signInOptions.SignIn.IsSignInByEmailEnabled && login.Contains('@'))
		{
			return await GetByConfirmedEmail(login, CancellationToken.None);
		}

		if (signInOptions.SignIn.IsSignInByPhoneNumberEnabled && login.StartsWith('+'))
		{
			return await GetByConfirmedPhoneNumber(login, CancellationToken.None);
		}

		if (signInOptions.SignIn.IsSignInByLoginEnabled)
		{
			return await FindByNameAsync(login);
		}

		return null;
	}

	public async Task<User> GetByConfirmedEmail(string email, CancellationToken cancellationToken)
	{
		email = NormalizeEmail(email);

		return await Users
			.FirstOrDefaultAsync(x => x.NormalizedEmail == email && x.EmailConfirmed, cancellationToken);
	}

	public async Task<User> GetByConfirmedPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
	{
		return await Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.PhoneNumberConfirmed,
			cancellationToken);
	}

	public async Task<User> FindByPhoneNumber(string phoneNumber, CancellationToken cancellationToken)
	{
		return await Users.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);
	}

	public virtual async Task<IList<Guid>> GetExistingUserIds(string email, string phoneNumber, string login)
	{
		return await Users
			.AsNoTracking()
			.Where(x =>
				(!string.IsNullOrEmpty(email) && x.NormalizedEmail == NormalizeEmail(email))
				|| (!string.IsNullOrEmpty(phoneNumber) && x.PhoneNumber == phoneNumber)
				|| (!string.IsNullOrEmpty(login) && x.NormalizedUserName == NormalizeName(login)))
			.Select(x => x.Id)
			.ToListAsync();
	}

	#endregion

	public double? GetMinutesTillLockoutEnd(User user)
	{
		var utcNow = _dateTimeProvider.GetUtcNow();
		var lockoutTime = user.LockoutEnd?.Subtract(utcNow).TotalMinutes;
		if (lockoutTime.HasValue)
		{
			lockoutTime = Math.Ceiling(lockoutTime.Value);
		}

		return lockoutTime;
	}

	public async Task<bool> ValidateTwoFactorCode(User user, string code)
	{
		if (user is null)
		{
			throw new ArgumentNullException(nameof(user));
		}

		var validTwoFactorProviders = await GetValidTwoFactorProvidersAsync(user);
		if (!validTwoFactorProviders.Any())
		{
			return false;
		}

		string provider = null;

		if (user.IsAuthenticatorEnabled && validTwoFactorProviders.Contains(Options.Tokens.AuthenticatorTokenProvider))
		{
			provider = TokenOptions.DefaultAuthenticatorProvider;
		}

		if (validTwoFactorProviders.Contains(TokenOptions.DefaultPhoneProvider) && provider is null)
		{
			provider = TokenOptions.DefaultPhoneProvider;
		}

		if (provider is null)
		{
			return false;
		}

		return await VerifyTwoFactorTokenAsync(user, provider, code);
	}

	public async Task ChangeAuthenticatorEnabled(User user, bool isEnabled)
	{
		user.IsAuthenticatorEnabled = isEnabled;

		var identityResult = !user.IsAuthenticatorEnabled
			? await RemoveAuthenticator(user)
			: await UpdateAsync(user);

		if (!identityResult.Succeeded)
		{
			var errorDescriptions = identityResult.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to update authenticator: {Error}", string.Join(", ", errorDescriptions));

			throw new ValidationException("Ошибка при обновлении аутентификатора пользователя.");
		}

		var changeDate = $"{_dateTimeProvider.GetUtcNow():yyyy-MM-dd HH:mm} (UTC)";
		var notificationsToSend = new List<NotificationBase>
		{
			new UserUpdateAuthenticatorStateNotification(Channels.Email)
			{
				RecipientId = user.Id,
				Recipient = user.Email,
				ChangeDate = changeDate,
				Action = isEnabled ? "включен" : "выключен",
				Username = user.UserName
			}
		};

		if (user.PhoneNumber is not null && user.PhoneNumberConfirmed)
		{
			var smsNotification = new UserUpdateAuthenticatorStateNotification(Channels.Sms)
			{
				Recipient = user.PhoneNumber,
				RecipientId = user.Id,
				ChangeDate = changeDate,
				Action = isEnabled ? "включен" : "выключен"
			};
			notificationsToSend.Add(smsNotification);
		}

		await _notificationManager.SendMultiple(notificationsToSend);
	}

	public async Task<DateTime?> GetLastPasswordChangedDate(User user)
	{
		var usedUserPasswords = await ((IApplicationUserStore)Store).GetLastUsedPasswords(user.Id, 1);
		var lastUserPassword = usedUserPasswords.FirstOrDefault();

		return lastUserPassword?.CreatedDate;
	}

	public Task<DateTime?> GetEventBlockEndDate(User user)
	{
		return ((IApplicationUserStore)Store).GetEventBlockEndDate(user.Id);
	}

	private async Task<bool> IsPreviousPassword(User user, string newPassword)
	{
		var applicationUserStore = (IApplicationUserStore)Store;
		var options = (RedRockIdentityOptions)Options;
		var passwordHistoryLength = options.Password.SamePasswordsCheckLimit;

		if (!passwordHistoryLength.HasValue)
		{
			return false;
		}

		var lastUsedPasswords =
			await applicationUserStore.GetLastUsedPasswords(user.Id, passwordHistoryLength.Value);

		return lastUsedPasswords
			.Any(x =>
				PasswordHasher.VerifyHashedPassword(user, x.PasswordHash, newPassword) !=
				PasswordVerificationResult.Failed);
	}

	private async Task<bool> CanChangePassword(User user)
	{
		var applicationUserStore = (IApplicationUserStore)Store;
		var options = (RedRockIdentityOptions)Options;
		var minChangePasswordHours = options.Password.MinHoursBetweenPasswordChange;

		if (!minChangePasswordHours.HasValue)
		{
			return true;
		}

		var previousPasswordHashes = await applicationUserStore.GetLastUsedPasswords(user.Id, 1);
		var lastPasswordHash = previousPasswordHashes.FirstOrDefault();
		if (lastPasswordHash is null)
		{
			return true;
		}

		return lastPasswordHash.CreatedDate.AddHours(minChangePasswordHours.Value) < _dateTimeProvider.GetUtcNow();
	}
}
