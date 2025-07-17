using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Features.GoogleAuth.Notification;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Models;
using RRMonitoring.Notification.ApiClients.Service.Notification.Rabbit;

namespace RRMonitoring.Identity.Application.Features.GoogleAuth.SendResetCode;

public class SendResetAuthenticatorCodeRequest : IRequest
{
	public Guid UserId { get; set; }

	public Channels Channel { get; set; }
}

public class SendResetAuthenticatorCodeRequestHandler : IRequestHandler<SendResetAuthenticatorCodeRequest, Unit>
{
	private readonly IdentityUserManager _userManager;
	private readonly IRabbitNotificationManager _notificationManager;

	public SendResetAuthenticatorCodeRequestHandler(
		IdentityUserManager userManager,
		IRabbitNotificationManager notificationManager)
	{
		_userManager = userManager;
		_notificationManager = notificationManager;
	}

	public async Task<Unit> Handle(SendResetAuthenticatorCodeRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"User with id: {request.UserId} not found");
		}

		var code = await _userManager.GenerateUserTokenAsync(
			user,
			TokenOptions.DefaultPhoneProvider,
			IdentityUserManager.ResetAuthenticatorPurpose);

		var notification = new ResetAuthenticatorNotification(request.Channel)
		{
			Recipient = request.Channel == Channels.Email ? user.Email : user.PhoneNumber,
			RecipientId = user.Id,
			Username = user.UserName,
			Code = code
		};
		await _notificationManager.SendMultiple(new NotificationBase[] { notification });

		return Unit.Value;
	}
}
