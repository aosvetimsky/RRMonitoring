namespace RRMonitoring.Identity.Application.Services.ExternalProviders.Models;

public class LoginProvider
{
	public string Name { get; set; }

	public string DisplayName { get; set; }

	public string IconUrl { get; set; }

	public LoginProvider(string name, string displayName, string iconUrl = null)
	{
		Name = name;
		DisplayName = displayName;
		IconUrl = iconUrl;
	}
}
