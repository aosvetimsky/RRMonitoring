using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.ResetPassword.Verify;

public class VerifyResetPasswordTokenRequestValidator : AbstractValidator<VerifyResetPasswordTokenRequest>
{
	public VerifyResetPasswordTokenRequestValidator()
	{
		RuleFor(x => x.UserId).NotEmpty();

		RuleFor(x => x.Token).NotEmpty();
	}
}
