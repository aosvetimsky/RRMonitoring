using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.GetByCurrentUser;

public class GetCurrentUserNotificationSettingsRequest : IRequest<IList<UserNotificationSettingResponse>>;

public class GetCurrentUserNotificationSettingsHandler(
	IRecipientSettingsRepository recipientSettingsRepository,
	IAccountService accountService,
	IMapper mapper)
	: IRequestHandler<GetCurrentUserNotificationSettingsRequest,
		IList<UserNotificationSettingResponse>>
{
	public async Task<IList<UserNotificationSettingResponse>> Handle(
		GetCurrentUserNotificationSettingsRequest request, CancellationToken cancellationToken)
	{
		var userId = accountService.GetCurrentUserId().ToString();
		var settings = await recipientSettingsRepository.GetByRecipientId(userId);

		return mapper.Map<IList<UserNotificationSettingResponse>>(settings);
	}
}
