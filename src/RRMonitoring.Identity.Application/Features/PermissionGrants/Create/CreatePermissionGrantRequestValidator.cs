using FluentValidation;
using Nomium.Core.Application.Services.DateTimeProvider;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Create;

public class CreatePermissionGrantRequestValidator : AbstractValidator<CreatePermissionGrantRequest>
{
	public CreatePermissionGrantRequestValidator(IDateTimeProvider dateTimeProvider)
	{
		RuleFor(x => x.SourceUserId)
			.NotEmpty();

		RuleFor(x => x.DestinationUserId)
			.NotEmpty();

		RuleFor(x => x.PermissionIds)
			.NotEmpty();

		RuleFor(x => x.GrantDates)
			.Must(x => x.EndDateTime > x.StartDateTime)
			.WithMessage("Дата конца периода делигирования должна быть больше даты начала");

		RuleFor(x => x.GrantDates)
			.Must(x => x.StartDateTime >= dateTimeProvider.GetUtcNow())
			.WithMessage("Дата начала периода делигирования должна быть больше текущей даты");

		RuleFor(x => x.Reason)
			.MaximumLength(256);
	}
}
