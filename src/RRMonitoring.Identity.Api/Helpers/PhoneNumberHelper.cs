using System.Text.RegularExpressions;

namespace RRMonitoring.Identity.Api.Helpers;

public static class PhoneNumberHelper
{
	public static string ModifyPhoneCountryCode(string phoneNumber)
	{
		return string.IsNullOrWhiteSpace(phoneNumber)
			? null
			: Regex.Replace(phoneNumber, "^8", "+7");
	}
}
