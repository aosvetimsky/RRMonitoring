using FluentValidation;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.SendCode;

internal class SendCodeToNewPhoneValidator : AbstractValidator<SendCodeToNewPhoneRequest>
{
	public SendCodeToNewPhoneValidator(IOptions<RegexesConfig> regexesConfig)
	{
		RuleFor(x => x.NewPhoneNumber)
			.NotEmpty()
			.Matches(regexesConfig.Value.PhoneNumberRegex)
			.WithMessage("Недействительнный номер телефона")
			.MaximumLength(20);
	}
}
