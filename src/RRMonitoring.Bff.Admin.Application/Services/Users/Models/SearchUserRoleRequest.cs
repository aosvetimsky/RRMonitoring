using Nomium.Core.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Users.Models;

public class SearchUserRoleRequest : PagedRequest
{
	public string Keyword { get; set; }
}
