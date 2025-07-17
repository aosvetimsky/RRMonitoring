using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.Users.Registration;

public class UserRegistrationValidator : AbstractValidator<UserRegistrationRequest>
{
	public UserRegistrationValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.Email).NotEmpty().WithMessage("Email не может быть пустым");
		RuleFor(x => x.Email)
			.Matches(regexesConfig.Value.EmailRegex, RegexOptions.IgnoreCase)
			.WithMessage("Неправильный формат email")
			.MaximumLength(100);

		RuleFor(x => x.UserName).NotEmpty().WithMessage("Имя пользователя не может быть пустым");
		RuleFor(x => x.UserName)
			.MinimumLength(4)
			.Matches(regexesConfig.Value.LoginRegex, RegexOptions.IgnoreCase)
			.WithMessage("Неправильный формат имени пользователя")
			.MaximumLength(30)
			.WithMessage("Имя пользователя должно быть короче 30 символов");

		RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль не может быть пустым");
		RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Подтвердите пароль");

		RuleFor(x => x.Password)
			.Must((x, password) => password == x.ConfirmPassword)
			.WithMessage("Пароли не совпадают");

		RuleFor(x => x.Password)
			.Matches(regexesConfig.Value.PasswordRegex, RegexOptions.IgnoreCase)
			.WithMessage("Пароль должен содержать заглавные и строчные буквы, цифры и символы.");
	}
}
