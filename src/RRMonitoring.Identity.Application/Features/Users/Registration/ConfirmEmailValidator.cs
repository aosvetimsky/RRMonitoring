using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.Users.Registration;

public class ConfirmEmailValidator : AbstractValidator<ConfirmUserEmailRequest>
{
	public ConfirmEmailValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.UserId).NotEmpty().WithMessage("Email не может быть пустым");
		RuleFor(x => x.Token).NotEmpty().WithMessage("Token не может быть пустым");
	}
}
