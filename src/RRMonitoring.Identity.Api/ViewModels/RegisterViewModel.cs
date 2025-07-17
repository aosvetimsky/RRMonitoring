using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Api.ViewModels;

public class RegisterViewModel
{
	[Required(ErrorMessage = "Имя пользователя не может быть пустым")]
	public string UserName { get; set; }

	[Required(ErrorMessage = "Email не может быть пустым")]
	[EmailAddress]
	public string Email { get; set; }

	[Required(ErrorMessage = "Пароль не может быть пустым")]
	public string Password { get; set; }

	[Required(ErrorMessage = "Повторите пароль")]
	public string ConfirmPassword { get; set; }

	[Required(ErrorMessage = "Подтвердите согласие с пользовательским соглашением")]
	public bool IsTermsConfirmed { get; set; }

	public string LoginUrl { get; set; }

	public string ReferralCode { get; set; }

	public IList<string> ErrorList { get; set; }
}
