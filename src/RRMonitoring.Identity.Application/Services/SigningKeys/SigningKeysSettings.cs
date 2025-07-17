using System;

namespace RRMonitoring.Identity.Application.Services.SigningKeys;

public class SigningKeysSettings
{
	public TimeSpan RetirementPeriod { get; set; }

	public TimeSpan ExpirationPeriod { get; set; }

	public TimeSpan ActivationPeriod { get; set; }

	public TimeSpan CachePeriod { get; set; }
}
