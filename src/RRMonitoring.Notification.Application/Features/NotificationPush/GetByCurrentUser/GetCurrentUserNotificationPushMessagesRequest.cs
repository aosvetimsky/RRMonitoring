using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Models;
using Nomium.Core.Security.Services.Account;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Enums;
using NotificationMessageDb = RRMonitoring.Notification.Domain.Entities.NotificationMessage;


namespace RRMonitoring.Notification.Application.Features.NotificationPush.GetByCurrentUser;

public class GetCurrentUserNotificationPushMessagesRequest
	: PagedRequest, IRequest<PagedList<UserNotificationPushMessageResponse>>;

public class GetCurrentUserNotificationPushMessagesHandler(
	INotificationMessageRepository notificationMessageRepository,
	IAccountService accountService)
	: IRequestHandler<GetCurrentUserNotificationPushMessagesRequest,
		PagedList<UserNotificationPushMessageResponse>>
{
	public async Task<PagedList<UserNotificationPushMessageResponse>> Handle(
		GetCurrentUserNotificationPushMessagesRequest request, CancellationToken cancellationToken)
	{
		var userId = accountService.GetCurrentUserId().ToString();

		var notificationMessages = await notificationMessageRepository.Search(new()
		{
			RecipientIds = [userId],
			Channels = [Channels.Push],
			IncludeNotification = true,
			Skip = request.Skip,
			Take = request.Take,
			SortExpressions =
			[
				new SortExpression { PropertyName = nameof(NotificationMessageDb.CreatedDate), Direction = SortDirection.Desc }
			],
		}, cancellationToken);

		return new PagedList<UserNotificationPushMessageResponse>
		{
			TotalCount = notificationMessages.TotalCount,
			Items = notificationMessages.Items
				.Select(x => new UserNotificationPushMessageResponse
				{
					Description = x.Notification.Description,
					CreatedDate = x.CreatedDate,
					NotificationBody = x.NotificationBody,
				})
				.ToList(),
		};
	}
}
