using FluentValidation;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Features.Notification.Send;

public sealed class SendNotificationRequestValidator
	: AbstractValidator<SendNotificationRequest>
{
	public SendNotificationRequestValidator()
	{
		RuleFor(x => x.Channel)
			.IsInEnum();

		When(x => x.Channel == Channels.Push,
			() =>
			{
				RuleFor(x => x.RecipientId)
					.NotNull();
			});

		RuleFor(x => x.Identifier)
			.NotNull();

		RuleFor(x => x.Recipient)
			.NotNull();
	}
}
