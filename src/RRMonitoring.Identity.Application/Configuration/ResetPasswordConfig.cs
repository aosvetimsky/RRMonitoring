namespace RRMonitoring.Identity.Application.Configuration;

public class ResetPasswordConfig
{
	public bool IsResetByLoginEnabled { get; set; }

	public bool IsResetByEmailEnabled { get; set; }

	public bool IsResetByMobileEnabled { get; set; }

	public ResetPasswordConfigEmailFlow EmailFlow { get; set; }
}
