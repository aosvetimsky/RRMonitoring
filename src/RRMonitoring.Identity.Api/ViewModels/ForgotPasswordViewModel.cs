using Microsoft.AspNetCore.Mvc;

namespace RRMonitoring.Identity.Api.ViewModels;

public record ForgotPasswordViewModel
{
	public string Error { get; set; }
	public string InfoMessage { get; set; }
	public int? UntilResend { get; set; }

	public string YandexSmartCaptchaClientSecret { get; set; }

	[FromForm(Name = "smart-token")]
	public string YandexCaptchaSmartToken { get; set; }

	[FromForm(Name = "login")]
	public string Login { get; set; }

	[FromForm(Name = "phone")]
	public string Phone { get; set; }

	[FromForm(Name = "type-email")]
	public bool IsLoginEmail { get; set; }
}
