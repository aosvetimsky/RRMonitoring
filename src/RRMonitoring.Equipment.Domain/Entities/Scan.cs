using System.Collections.Generic;
using Nomium.Core.Data.Entities;

namespace RRMonitoring.Equipment.Domain.Entities;

public class Scan : AuditableEntity
{
	public string Name { get; set; }

	public string IpRangeDefinition { get; set; }

	public byte StatusId { get; set; }
	public ScanStatus Status { get; set; }

	public ICollection<ScanResult> ScanResults { get; set; } = new List<ScanResult>();
}
