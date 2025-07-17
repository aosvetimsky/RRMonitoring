using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.ResetPassword;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
	public ResetPasswordRequestValidator()
	{
		RuleFor(x => x.NewPassword)
			.NotEmpty();

		RuleFor(x => x.Token)
			.NotEmpty();
	}
}
