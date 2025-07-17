using Nomium.Core.FileStorage.Providers.S3.Configuration;

namespace RRMonitoring.Equipment.Application.Configuration;

public class EquipmentS3FileProviderConfiguration : S3FileProviderConfiguration
{
	public string FirmwareFolder { get; set; }
}
