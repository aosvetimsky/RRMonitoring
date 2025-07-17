using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone;

public class UpdateCurrentUserPhoneValidator : AbstractValidator<UpdateCurrentUserPhoneRequest>
{
	public UpdateCurrentUserPhoneValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.TwoFactorPhoneCode)
			.NotEmpty()
			.MinimumLength(6)
			.WithMessage("Недействительный код подтверждения")
			.When(x => x.TwoFactorPhoneCode is not null);

		RuleFor(x => x.NewPhoneCode)
			.NotEmpty()
			.MinimumLength(6)
			.WithMessage("Недействительный код подтверждения");

		RuleFor(x => x.NewPhoneNumber)
			.NotEmpty()
			.Matches(regexesConfig.Value.PhoneNumberRegex)
			.MaximumLength(20)
			.WithMessage("Недействительнный номер телефона");
	}
}
