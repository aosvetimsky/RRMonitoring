namespace RRMonitoring.Identity.Domain.Configuration;

public abstract class ExternalProviderBaseConfiguration
{
	public bool IsEnabled { get; set; }
	public bool IsUserRegistrationEnabled { get; set; }

	public string DisplayName { get; set; }

	public string ClientId { get; set; }
	public string ClientSecret { get; set; }

	public string IconUrl { get; set; }
}
