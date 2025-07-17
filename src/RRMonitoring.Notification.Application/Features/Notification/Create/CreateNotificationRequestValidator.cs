using FluentValidation;

namespace RRMonitoring.Notification.Application.Features.Notification.Create;

public sealed class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequest>
{
	public CreateNotificationRequestValidator()
	{
		RuleFor(x => x.Identifier)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty()
			.MaximumLength(250);

		RuleFor(x => x.GroupId)
			.NotEmpty();

		RuleForEach(x => x.Templates)
			.SetValidator(new CreateNotificationTemplateItemValidator());
	}
}

public sealed class CreateNotificationTemplateItemValidator : AbstractValidator<CreateNotificationTemplateItem>
{
	public CreateNotificationTemplateItemValidator()
	{
		RuleFor(x => x.ChannelId)
			.NotEmpty()
			.IsInEnum();
	}
}
