namespace RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

public sealed record ForgotPasswordRequest
{
	public string Login { get; init; }

	public string SmartToken { get; init; }

	public bool IsViaEmail { get; init; }
}
