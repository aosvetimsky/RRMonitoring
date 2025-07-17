using System.Diagnostics.CodeAnalysis;

namespace RRMonitoring.Notification.Application.Configuration.Providers;

public class FirebasePushProviderConfiguration
{
	public PrivateKey PrivateKey { get; set; }
}

// ReSharper disable InconsistentNaming
[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Used for serialization")]
public class PrivateKey
{
	public string Type { get; set; }
	public string Project_id { get; set; }
	public string Private_key_id { get; set; }
	public string Private_key { get; set; }
	public string Client_email { get; set; }
	public string Client_id { get; set; }
	public string Auth_uri { get; set; }
	public string Token_uri { get; set; }
	public string Auth_provider_x509_cert_url { get; set; }
	public string Client_x509_cert_url { get; set; }
}
