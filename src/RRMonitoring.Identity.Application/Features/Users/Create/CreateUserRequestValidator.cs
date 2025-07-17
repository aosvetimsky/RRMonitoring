using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.Users.Create;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
	public CreateUserRequestValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MaximumLength(20);

		RuleFor(x => x.LastName)
			.MaximumLength(50);

		RuleFor(x => x.MiddleName)
			.MaximumLength(50);

		RuleFor(x => x.PhoneNumber)
			.Matches(regexesConfig.Value.PhoneNumberRegex)
			.WithMessage("Номер телефона имеет неверный формат.");

		RuleFor(x => x.Email)
			.NotEmpty()
			.Matches(regexesConfig.Value.EmailRegex, RegexOptions.IgnoreCase)
			.WithMessage("Email имеет неверный формат.")
			.MaximumLength(100);

		RuleFor(x => x.Login)
			.MinimumLength(1)
			.Matches(regexesConfig.Value.LoginRegex, RegexOptions.IgnoreCase)
			.WithMessage("Login имеет неверный формат.")
			.MaximumLength(100);
	}
}
