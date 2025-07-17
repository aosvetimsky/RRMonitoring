using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.SendCode;

internal class SendCodeToNewUserEmailValidator : AbstractValidator<SendCodeToNewUserEmailRequest>
{
	public SendCodeToNewUserEmailValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.NewEmail)
			.NotEmpty()
			.Matches(regexesConfig.Value.EmailRegex, RegexOptions.IgnoreCase)
			.WithMessage("Email has wrong format")
			.MaximumLength(100);
	}
}
