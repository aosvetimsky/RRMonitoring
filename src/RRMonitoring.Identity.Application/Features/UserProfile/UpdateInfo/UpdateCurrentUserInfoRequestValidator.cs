using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateInfo;

public class UpdateCurrentUserInfoRequestValidator : AbstractValidator<UpdateCurrentUserInfoRequest>
{
	public UpdateCurrentUserInfoRequestValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MaximumLength(20);

		RuleFor(x => x.LastName)
			.MaximumLength(50);

		RuleFor(x => x.TelegramLogin)
			.MaximumLength(50);

		RuleFor(x => x.PhoneNumber)
			.Matches(regexesConfig.Value.PhoneNumberRegex)
			.WithMessage("Номер телефона имеет неверный формат.");

		RuleFor(x => x.Email)
			.Matches(regexesConfig.Value.EmailRegex, RegexOptions.IgnoreCase)
			.WithMessage("Email имеет неверный формат.")
			.MaximumLength(100);
	}
}
