using Nomium.Core.Data.Entities;

namespace RRMonitoring.Identity.Domain.Entities;

public class Country : EntityBase<int>
{
	public string Name { get; set; }

	public string Code { get; set; }

	public bool IsActive { get; set; }
}
