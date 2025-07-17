using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.ExternalProviders;

public interface IExternalUserFactory
{
	string GetUserExternalId(string provider, IEnumerable<Claim> claims);

	bool IsUserRegistrationEnabled(string provider);

	Task<ExternalUser> GetByProvider(string provider, string externalUserId);
}
