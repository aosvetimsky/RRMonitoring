using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Pools.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools;

public interface IPoolService
{
	Task<PoolByIdResponse> GetById(Guid id, CancellationToken cancellationToken);

	Task<PagedList<SearchPoolsResponseItem>> Search(SearchPoolsRequest request, CancellationToken cancellationToken);

	Task<Guid> Create(CreatePoolRequest request, CancellationToken cancellationToken);

	Task<Guid> Update(UpdatePoolRequest request, CancellationToken cancellationToken);

	Task Delete(Guid id, CancellationToken cancellationToken);
}
