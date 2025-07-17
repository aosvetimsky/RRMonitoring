using FluentValidation;
using RRMonitoring.Identity.Application.Features.Tenants.Create;

namespace RRMonitoring.Identity.Application.Features.Tenants.Update;

public class UpdateTenantRequestValidator : AbstractValidator<CreateTenantRequest>
{
	public UpdateTenantRequestValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(50);
	}
}
