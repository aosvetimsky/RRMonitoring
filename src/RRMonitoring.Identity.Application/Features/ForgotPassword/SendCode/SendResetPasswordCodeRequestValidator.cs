using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendCode;

public class SendResetPasswordCodeRequestValidator : AbstractValidator<SendResetPasswordCodeRequest>
{
	public SendResetPasswordCodeRequestValidator()
	{
		RuleFor(x => x.User).NotNull();
	}
}
