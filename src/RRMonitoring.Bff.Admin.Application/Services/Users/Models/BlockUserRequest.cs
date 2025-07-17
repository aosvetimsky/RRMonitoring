namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class BlockUserRequest
{
	public string BlockReason { get; set; }

	public bool RemovePassword { get; set; }
}
