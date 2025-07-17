using System;

namespace RRMonitoring.Identity.Application.Features.Tenants.GetByCode;

public class TenantByCodeResponse
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }
}
