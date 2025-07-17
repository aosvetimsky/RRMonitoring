namespace RRMonitoring.Identity.Application.Features.Users.ChangeStatus;

public class ChangeUserBlockRequest
{
	public string BlockReason { get; set; }

	public bool RemovePassword { get; set; }
}
