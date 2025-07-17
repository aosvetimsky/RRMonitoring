using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Workers.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Workers;

public interface IWorkerService
{
	Task<PagedList<SearchWorkersResponse>> Search(SearchWorkersRequest request, CancellationToken cancellationToken);
}
