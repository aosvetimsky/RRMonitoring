using Microsoft.AspNetCore.Identity;

namespace RRMonitoring.Identity.Application.Configuration;

public class RedRockSignInOptions : SignInOptions
{
	public bool IsSignInByLoginEnabled { get; set; }

	public bool IsSignInByEmailEnabled { get; set; }

	public bool IsSignInByPhoneNumberEnabled { get; set; }
}
