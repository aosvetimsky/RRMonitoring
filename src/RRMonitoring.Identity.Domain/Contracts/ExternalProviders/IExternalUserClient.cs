using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Domain.Contracts.ExternalProviders;

public interface IExternalUserClient
{
	string GetUserId(IEnumerable<Claim> claims);

	bool IsUserRegistrationEnabled();

	Task<ExternalUser> GetUser(string userId);
}
