namespace RRMonitoring.Identity.Application.Configuration;

public class ReferralCodeSettingsConfiguration
{
	public int MaxAttempts { get; set; }
	public int AmountOfCodes { get; set; }
	public int CodeLength { get; set; }
}
