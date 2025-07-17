using Microsoft.AspNetCore.Identity;

namespace RRMonitoring.Identity.Application.Configuration;

public class RedRockPasswordOptions : PasswordOptions
{
	/// <summary>
	/// This property should be null if you don't want to validate
	/// characters from login and name in password
	/// </summary>
	public int? MaxMatchingCredentialSymbolSequences { get; set; }

	/// <summary>
	/// This property should be null if you don't want to check password history
	/// otherwise count of passwords in the history which will be validated
	/// on password change
	/// </summary>
	public int? SamePasswordsCheckLimit { get; set; }

	/// <summary>
	/// This property should be null if you don't want to restrict
	/// min hours between password change otherwise set hours
	/// which should be taken between password changing
	/// </summary>
	public int? MinHoursBetweenPasswordChange { get; set; }

	/// <summary>
	/// This property should be null if you don't want automatic
	/// password expiration otherwise set days after which user should
	/// change his password
	/// </summary>
	public int? DaysAfterPasswordChangeRequired { get; set; }
}
