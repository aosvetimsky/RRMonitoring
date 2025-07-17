namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail;

public sealed class UpdateCurrentUserEmailResponse
{
	public string Token { get; set; }
	public int ResendCodeTimeout { get; set; }
}
