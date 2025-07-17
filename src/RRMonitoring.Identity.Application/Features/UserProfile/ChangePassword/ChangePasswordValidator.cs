using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
{
	public ChangePasswordValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.TwoFactorCode)
			.NotEmpty()
			.MinimumLength(6)
			.WithMessage("Недействительный код подтверждения");

		RuleFor(x => x.OldPassword)
			.NotEmpty();

		RuleFor(x => x.NewPassword)
			.NotEmpty()
			.Matches(regexesConfig.Value.PasswordRegex)
			.WithMessage("Пароль не соответствует требованиям");

		RuleFor(x => x)
			.Must(x => x.NewPassword == x.ConfirmedNewPassword)
			.WithMessage("Введенные пароли не совпадают");
	}
}
