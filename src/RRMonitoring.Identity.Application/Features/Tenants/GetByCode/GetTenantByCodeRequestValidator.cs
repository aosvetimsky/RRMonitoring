using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Tenants.GetByCode;

public class GetTenantByCodeRequestValidator : AbstractValidator<GetTenantByCodeRequest>
{
	public GetTenantByCodeRequestValidator()
	{
		RuleFor(x => x.Code)
			.NotEmpty();
	}
}
