using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Api.ViewModels;

public class TwoFactorAuthViewModel
{
	[Required(ErrorMessage = "Поле 'Код' не может быть пустым")]
	public string Code { get; set; }

	public string ReturnUrl { get; set; }

	public bool DisplayAgreementModal { get; set; }

	public string AgreementUrl { get; set; }

	public IList<string> ErrorList { get; set; }
}
