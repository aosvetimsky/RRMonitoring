namespace RRMonitoring.Identity.Application.Services.Agreement;

internal class SignedVerifiedLogin
{
	public VerifiedLogin Login { get; set; }

	public string Signature { get; set; }
}
