using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail;

internal sealed class UpdateCurrentUserEmailValidator : AbstractValidator<UpdateCurrentUserEmailRequest>
{
	public UpdateCurrentUserEmailValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.NewEmail)
			.NotEmpty()
			.Matches(regexesConfig.Value.EmailRegex, RegexOptions.IgnoreCase)
			.WithMessage("Email has wrong format")
			.MaximumLength(100);

		RuleFor(x => x.NewEmailCode)
			.NotEmpty()
			.MinimumLength(6)
			.WithMessage("Недействительный email код");

		RuleFor(x => x.TwoFactorCode)
			.NotEmpty()
			.MinimumLength(6)
			.WithMessage("Недействительный 2FA код");
	}
}
