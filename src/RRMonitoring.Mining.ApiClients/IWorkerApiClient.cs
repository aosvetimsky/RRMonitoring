using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Mining.ApiClients;

public interface IWorkerApiClient
{
	[Post("/v1/worker/search")]
	Task<PagedList<SearchWorkersResponseDto>> Search([Body] SearchWorkersRequestDto requestDto, CancellationToken cancellationToken);
}
