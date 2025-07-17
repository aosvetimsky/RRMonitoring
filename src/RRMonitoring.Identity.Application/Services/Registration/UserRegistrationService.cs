using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.NotificationHistory;
using RRMonitoring.Identity.Application.Services.Registration.Notification;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Services.Registration;

public class UserRegistrationService : IUserRegistrationService
{
	private readonly IHttpNotificationManager _httpNotificationManager;
	private readonly IIdentityNotificationHistoryService _notificationHistoryService;
	private readonly int _resendTimeout;

	public UserRegistrationService(
		IHttpNotificationManager httpNotificationManager,
		IIdentityNotificationHistoryService notificationHistoryService,
		IOptions<TimeoutConfig> timeoutConfig)
	{
		_httpNotificationManager = httpNotificationManager;
		_notificationHistoryService = notificationHistoryService;

		_resendTimeout = timeoutConfig.Value.SendRegistrationCodeTimeout;
	}

	public async Task<int> GetRegistrationLinkTimeout(Guid userId)
	{
		return await _notificationHistoryService.GetTimeout<UserRegistrationNotification>(userId, _resendTimeout);
	}

	public async Task SendLink(User user, string link)
	{
		var userRegistrationNotification = new UserRegistrationNotification
		{
			Recipient = user.Email,
			RecipientId = user.Id,
			Link = link,
			Username = user.UserName,
		};

		await _httpNotificationManager.SendEmail(userRegistrationNotification);
	}
}
