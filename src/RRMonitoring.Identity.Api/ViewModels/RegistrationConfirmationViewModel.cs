namespace RRMonitoring.Identity.Api.ViewModels;

public class RegistrationConfirmationViewModel
{
	public string Email { get; set; }

	public string LoginUrl { get; set; }

	public bool IsTokenValid { get; set; }
}
