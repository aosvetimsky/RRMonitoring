using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Workers.Models;
using RRMonitoring.Mining.ApiClients;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Bff.Admin.Application.Services.Workers;

public class WorkerService(IWorkerApiClient workerApiClient, IMapper mapper) : IWorkerService
{
	public async Task<PagedList<SearchWorkersResponse>> Search(SearchWorkersRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchWorkersRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "CreatedDate", Direction = SortDirection.Desc }
		};

		var workers = await workerApiClient.Search(requestDto, cancellationToken);

		var workersResponse = mapper.Map<PagedList<SearchWorkersResponse>>(workers);

		foreach (var worker in workersResponse.Items)
		{
			worker.EquipmentQuantity = 0; // TODO take for equipment service
			worker.EquipmentUnderRepairQuantity = 0; // TODO take for equipment service
			worker.TotalHashrate = 0; // TODO take for equipment service
		}

		return workersResponse;
	}
}
