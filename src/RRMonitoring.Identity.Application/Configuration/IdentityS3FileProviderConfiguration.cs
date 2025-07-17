using Nomium.Core.FileStorage.Providers.S3.Configuration;

namespace RRMonitoring.Identity.Application.Configuration;

public class IdentityS3FileProviderConfiguration : S3FileProviderConfiguration
{
	public string UserPhotoFolder { get; set; }
}
