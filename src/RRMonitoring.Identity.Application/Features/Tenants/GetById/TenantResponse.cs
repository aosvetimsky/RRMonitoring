using System;

namespace RRMonitoring.Identity.Application.Features.Tenants.GetById;

public class TenantResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}
