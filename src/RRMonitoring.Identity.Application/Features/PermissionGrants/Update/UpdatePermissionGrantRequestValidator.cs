using FluentValidation;
using Nomium.Core.Application.Services.DateTimeProvider;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Update;

public class UpdatePermissionGrantRequestValidator : AbstractValidator<UpdatePermissionGrantRequest>
{
	public UpdatePermissionGrantRequestValidator(IDateTimeProvider dateTimeProvider)
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.GrantDates)
			.Must(x => x.EndDateTime > x.StartDateTime)
			.WithMessage("Дата конца периода делигирования должна быть больше даты начала");

		RuleFor(x => x.GrantDates)
			.Must(x => x.StartDateTime >= dateTimeProvider.GetUtcNow())
			.WithMessage("Дата начала периода делигирования должна быть больше текущей даты");

		RuleFor(x => x.PermissionIds)
			.NotEmpty();
	}
}
