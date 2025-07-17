using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class Tenant : EntityBase
{
	public string Name { get; set; }

	public string Code { get; set; }
}
