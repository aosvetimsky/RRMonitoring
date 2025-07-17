namespace RRMonitoring.Identity.Application.Features.ResetPassword;

public class ResetPasswordResult
{
	public bool IsSuccess { get; set; }

	public string ErrorMessage { get; set; }

	public static ResetPasswordResult Failed(string errorMessage)
	{
		return new ResetPasswordResult
		{
			ErrorMessage = errorMessage
		};
	}

	public static ResetPasswordResult Succeed()
	{
		return new ResetPasswordResult
		{
			IsSuccess = true
		};
	}
}
