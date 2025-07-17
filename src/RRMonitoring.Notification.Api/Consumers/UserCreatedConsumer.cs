using System.Threading.Tasks;
using MassTransit;
using MediatR;
using RRMonitoring.Identity.BusEvents.Users;
using RRMonitoring.Notification.Application.Features.RecipientSettings.AddDefault;

namespace RRMonitoring.Notification.Api.Consumers;

public class UserCreatedConsumer(IMediator mediator) : IConsumer<UserCreatedEvent>
{
	public Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		return mediator.Send(new AddDefaultUserSettingsRequest(context.Message.Id));
	}
}
