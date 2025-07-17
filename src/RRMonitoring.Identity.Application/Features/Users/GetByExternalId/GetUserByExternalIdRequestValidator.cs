using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Users.GetByExternalId;

public class GetUserByExternalIdRequestValidator : AbstractValidator<GetUserByExternalIdRequest>
{
	public GetUserByExternalIdRequestValidator()
	{
		RuleFor(x => x.ExternalId)
			.NotEmpty();
	}
}
