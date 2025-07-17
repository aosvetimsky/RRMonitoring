using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.Users.GetById;

internal class GetUserByIdRequestValidator : AbstractValidator<GetUserByIdRequest>
{
	public GetUserByIdRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
