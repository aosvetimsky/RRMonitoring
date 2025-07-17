using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateUsername;

public class UpdateCurrentUserUsernameRequestValidator : AbstractValidator<UpdateCurrentUserUsernameRequest>
{
	public UpdateCurrentUserUsernameRequestValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.NewUsername)
			.NotEmpty()
			.Matches(regexesConfig.Value.LoginRegex, RegexOptions.IgnoreCase)
			.WithMessage("Username has wrong format")
			.MaximumLength(12)
			.WithMessage("Username should be not more than 12 symbols");

		RuleFor(x => x.CurrentPassword)
			.NotEmpty();
	}
}
