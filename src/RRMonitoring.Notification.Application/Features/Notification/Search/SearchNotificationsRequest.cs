using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Models;

namespace RRMonitoring.Notification.Application.Features.Notification.Search;

public sealed class SearchNotificationsRequest : PagedRequest, IRequest<PagedList<SearchNotificationsResponse>>
{
	public string Keyword { get; set; }
}

public sealed class SearchNotificationsHandler(
	INotificationRepository notificationRepository,
	IMapper mapper)
	: IRequestHandler<SearchNotificationsRequest, PagedList<SearchNotificationsResponse>>
{
	public async Task<PagedList<SearchNotificationsResponse>> Handle(
		SearchNotificationsRequest request, CancellationToken cancellationToken)
	{
		var criteria = mapper.Map<SearchNotificationsCriteria>(request);
		var notifications = await notificationRepository.Search(criteria);

		return mapper.Map<PagedList<SearchNotificationsResponse>>(notifications);
	}
}
