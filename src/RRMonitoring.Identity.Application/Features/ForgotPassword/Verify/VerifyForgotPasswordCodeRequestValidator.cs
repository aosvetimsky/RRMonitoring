using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.Verify;

public class VerifyForgotPasswordCodeRequestValidator : AbstractValidator<VerifyForgotPasswordCodeRequest>
{
	public VerifyForgotPasswordCodeRequestValidator()
	{
		RuleFor(x => x.Code).NotEmpty();

		RuleFor(x => x.UserId).NotEmpty();
	}
}
