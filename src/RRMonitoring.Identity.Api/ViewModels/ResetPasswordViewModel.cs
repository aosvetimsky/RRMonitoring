using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Api.ViewModels;

public class ResetPasswordViewModel
{
	[DataType(DataType.Password)]
	public string NewPassword { get; set; }

	[DataType(DataType.Password)]
	public string ConfirmedNewPassword { get; set; }

	public IList<string> ErrorList { get; set; }

	public string LoginPageUrl { get; set; }

	public ResetPasswordViewModel()
	{
	}

	public ResetPasswordViewModel(string loginPageUrl)
	{
		LoginPageUrl = loginPageUrl;
	}

	public ResetPasswordViewModel(string error, string loginPageUrl) : this(new List<string> { error }, loginPageUrl)
	{
	}

	public ResetPasswordViewModel(IList<string> errorList, string loginPageUrl)
	{
		LoginPageUrl = loginPageUrl;
		ErrorList = errorList;
	}
}
