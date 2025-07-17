using FluentValidation;

namespace RRMonitoring.Notification.Application.Features.RecipientSettings.Set;

public sealed class CreateRecipientSettingsListValidator : AbstractValidator<SetRecipientSettingsRequest>
{
	public CreateRecipientSettingsListValidator()
	{
		RuleForEach(x => x.SetRecipientSettings)
			.SetValidator(new SetRecipientSettingsValidator());
	}
}

public sealed class SetRecipientSettingsValidator : AbstractValidator<SetRecipientSettings>
{
	public SetRecipientSettingsValidator()
	{
		RuleFor(x => x.Channel)
			.IsInEnum();

		RuleFor(x => x.RecipientId)
			.NotEmpty();

		RuleFor(x => x.NotificationIdentifier)
			.NotEmpty();
	}
}
