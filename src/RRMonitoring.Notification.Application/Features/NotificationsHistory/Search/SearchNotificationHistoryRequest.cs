using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Enums;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Application.Features.NotificationsHistory.Search;

public class SearchNotificationHistoryRequest : PagedRequest, IRequest<PagedList<SearchNotificationHistoryResponse>>
{
	public string Keyword { get; set; }
	public IList<Channels> Channels { get; set; }
	public DateTimePeriod? DatePeriod { get; set; }
	public IList<NotificationStatuses> Statuses { get; set; }
}

public class SearchNotificationHistoryHandler(
	INotificationMessageHistoryRepository notificationMessageHistoryRepository,
	IMapper mapper)
	: IRequestHandler<SearchNotificationHistoryRequest,
		PagedList<SearchNotificationHistoryResponse>>
{
	public async Task<PagedList<SearchNotificationHistoryResponse>> Handle(
		SearchNotificationHistoryRequest request, CancellationToken cancellationToken)
	{
		var criteria = mapper.Map<SearchNotificationMessageHistoryCriteria>(request);
		var history = await notificationMessageHistoryRepository.Search(criteria);

		return mapper.Map<PagedList<SearchNotificationHistoryResponse>>(history);
	}
}
