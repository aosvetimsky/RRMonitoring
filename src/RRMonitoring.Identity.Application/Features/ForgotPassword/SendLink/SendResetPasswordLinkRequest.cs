using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendLink;

public class SendResetPasswordLinkRequest : IRequest
{
	public string Email { get; set; }

	public SendResetPasswordLinkRequest(string email)
	{
		Email = email;
	}
}

public class SendResetPasswordLinkHandler : IRequestHandler<SendResetPasswordLinkRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly ILinkService _linkService;
	private readonly IRabbitNotificationManager _notificationManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;

	private readonly int _resendTimeout;

	public SendResetPasswordLinkHandler(
		IdentityUserManager userManager,
		ILinkService linkService,
		IRabbitNotificationManager notificationManager,
		IIdentityNotificationHistoryService notificationHistoryService,
		IOptions<TimeoutConfig> timeoutOptions)
	{
		_userManager = userManager;
		_linkService = linkService;
		_notificationManager = notificationManager;
		_notificationHistoryService = notificationHistoryService;

		_resendTimeout = timeoutOptions.Value.SendResetPasswordCodeTimeout;
	}

	public async Task<Unit> Handle(SendResetPasswordLinkRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByEmailAsync(request.Email);
		if (user is null)
		{
			throw new ValidationException($"Пользователь с почтой: '{request.Email}' не найден.");
		}

		var secondsTillNextSend =
			await _notificationHistoryService.GetTimeout<ResetPasswordEmailNotification>(user.Id, _resendTimeout);
		if (secondsTillNextSend != 0)
		{
			throw new ValidationException(
				$"Повторная отправка письма возможна только через: {secondsTillNextSend} секунд");
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		var link = _linkService.GenerateResetPasswordLink(user.Id, token);

		var resetPasswordEmailNotification = new ResetPasswordEmailNotification
		{
			Username = user.UserName, Link = link, Recipient = user.Email, RecipientId = user.Id
		};

		await _notificationManager.SendEmail(resetPasswordEmailNotification);

		return Unit.Value;
	}
}
