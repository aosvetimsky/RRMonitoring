using Microsoft.AspNetCore.Identity;

namespace RRMonitoring.Identity.Application.Configuration;

public class RedRockIdentityOptions : IdentityOptions
{
	public new RedRockPasswordOptions Password { get; set; }

	public new RedRockSignInOptions SignIn { get; set; }
}
