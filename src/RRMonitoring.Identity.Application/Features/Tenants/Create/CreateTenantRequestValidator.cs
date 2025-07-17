using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Tenants.Create;

public class CreateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
{
	public CreateTenantRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(50);
	}
}
