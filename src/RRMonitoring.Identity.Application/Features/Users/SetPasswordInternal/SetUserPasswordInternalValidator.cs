using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.Users.SetPasswordInternal;

public class SetUserPasswordInternalValidator : AbstractValidator<SetUserPasswordInternalRequest>
{
	public SetUserPasswordInternalValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.Id)
			.NotEmpty();

		RuleFor(x => x.Password)
			.NotEmpty()
			.Matches(regexesConfig.Value.PasswordRegex)
			.WithMessage("Пароль не соответствует требованиям");
	}
}
