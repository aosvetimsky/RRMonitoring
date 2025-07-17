using FluentValidation;

namespace RRMonitoring.Notification.Application.Features.Notification.Update;

public sealed class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
{
	public UpdateNotificationRequestValidator()
	{
		RuleFor(x => x.Identifier)
			.NotEmpty();

		RuleFor(x => x.Description)
			.NotEmpty()
			.MaximumLength(250);

		RuleFor(x => x.GroupId)
			.NotEmpty();

		RuleForEach(x => x.Templates)
			.SetValidator(new UpdateNotificationTemplateItemValidator());
	}
}

public sealed class UpdateNotificationTemplateItemValidator : AbstractValidator<UpdateNotificationTemplateItem>
{
	public UpdateNotificationTemplateItemValidator()
	{
		RuleFor(x => x.ChannelId)
			.NotEmpty()
			.IsInEnum();
	}
}
