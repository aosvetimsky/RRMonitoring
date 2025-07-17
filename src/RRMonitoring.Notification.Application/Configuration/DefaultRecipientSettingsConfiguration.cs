using System.Collections.Generic;

namespace RRMonitoring.Notification.Application.Configuration;

public class DefaultRecipientSettingsConfiguration
{
	public Dictionary<string, DefaultSettingState> Values { get; set; } = new();
}

public sealed record DefaultSettingState(bool? IsEmailEnabled, bool? IsPhoneEnabled);
