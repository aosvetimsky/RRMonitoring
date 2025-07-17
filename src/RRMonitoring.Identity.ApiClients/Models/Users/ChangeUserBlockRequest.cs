namespace RRMonitoring.Identity.ApiClients.Models.Users;

public class ChangeUserBlockRequest
{
	public string BlockReason { get; set; }

	public bool RemovePassword { get; set; }
}
