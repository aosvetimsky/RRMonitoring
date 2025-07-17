using System.Collections.Generic;
using RRMonitoring.Identity.Application.Services.ExternalProviders.Models;

namespace RRMonitoring.Identity.Application.Services.ExternalProviders;

public interface IExternalProviderService
{
	List<LoginProvider> GetActiveDirectoryProviders();
}
