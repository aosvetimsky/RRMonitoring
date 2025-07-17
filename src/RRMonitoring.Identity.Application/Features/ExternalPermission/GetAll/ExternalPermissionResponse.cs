using System.ComponentModel.DataAnnotations;

namespace RRMonitoring.Identity.Application.Features.ExternalPermission.GetAll;

public class ExternalPermissionResponse
{
	[Required]
	public byte Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public string Code { get; set; }
}
