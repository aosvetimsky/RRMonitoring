using Nomium.Core.ApiClient.Configuration;

namespace RRMonitoring.Bff.Admin.Application.Configurations;

public class ApiClientConfigurations
{
	public ApiClientConfiguration ColocationService { get; set; }

	public ApiClientConfiguration EquipmentService { get; set; }

	public ApiClientConfiguration IdentityService { get; set; }

	public ApiClientConfiguration MiningService { get; set; }
}
