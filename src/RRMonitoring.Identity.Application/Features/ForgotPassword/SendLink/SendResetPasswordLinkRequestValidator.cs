using FluentValidation;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.SendLink;

public class SendResetPasswordLinkRequestValidator : AbstractValidator<SendResetPasswordLinkRequest>
{
	public SendResetPasswordLinkRequestValidator()
	{
		RuleFor(x => x.Email).NotEmpty();
	}
}
