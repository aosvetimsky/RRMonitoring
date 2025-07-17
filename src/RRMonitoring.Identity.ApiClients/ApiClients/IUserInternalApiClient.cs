using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using RRMonitoring.Identity.ApiClients.Models.Users;

namespace RRMonitoring.Identity.ApiClients.ApiClients;

public interface IUserInternalApiClient
{
	[Post("/internal/user/get-by-ids")]
	Task<List<UserByIdResponse>> GetByIds([Body] IEnumerable<Guid> ids, CancellationToken cancellationToken);
}
