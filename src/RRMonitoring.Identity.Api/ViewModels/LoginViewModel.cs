using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Services.ExternalProviders.Models;

namespace RRMonitoring.Identity.Api.ViewModels;

public class LoginViewModel
{
	[Required(ErrorMessage = "Поле 'Email или телефон' не может быть пустым")]
	public string Login { get; set; }

	[Required(ErrorMessage = "Поле 'Пароль' не может быть пустым")]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	public IList<string> ErrorList { get; set; }

	public string ReturnUrl { get; set; }

	public bool DisplayAgreementModal { get; set; }

	public string AgreementUrl { get; set; }

	public string RegistrationUrl { get; set; }

	public IList<LoginProvider> AdProviders { get; set; }

	public string YandexSmartCaptchaClientSecret { get; set; }

	[FromForm(Name = "smart-token")]
	public string YandexCaptchaSmartToken { get; set; }
}
