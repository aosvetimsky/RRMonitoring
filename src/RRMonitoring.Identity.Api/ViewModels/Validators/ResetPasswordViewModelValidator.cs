using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Api.ViewModels.Validators;

public class ResetPasswordViewModelValidator : AbstractValidator<ResetPasswordViewModel>
{
	public ResetPasswordViewModelValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.NewPassword)
			.NotEmpty()
			.Matches(regexesConfig.Value.PasswordRegex)
			.WithMessage("Пароль не соответствует требованиям");

		RuleFor(x => x)
			.Must(x => x.NewPassword == x.ConfirmedNewPassword)
			.WithMessage("Введенные пароли не совпадают");
	}
}
