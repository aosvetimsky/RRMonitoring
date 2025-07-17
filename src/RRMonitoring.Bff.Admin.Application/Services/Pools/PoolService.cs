using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Pools.Models;
using RRMonitoring.Mining.ApiClients;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Bff.Admin.Application.Services.Pools;

public class PoolService(IPoolApiClient poolApiClient, IMapper mapper) : IPoolService
{
	public async Task<PoolByIdResponse> GetById(Guid id, CancellationToken cancellationToken)
	{
		var pool = await poolApiClient.GetById(id, cancellationToken);

		return mapper.Map<PoolByIdResponse>(pool);
	}

	public async Task<PagedList<SearchPoolsResponseItem>> Search(SearchPoolsRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchPoolsRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "CreatedDate", Direction = SortDirection.Desc }
		};

		var pools = await poolApiClient.Search(requestDto, cancellationToken);

		return mapper.Map<PagedList<SearchPoolsResponseItem>>(pools);
	}

	public async Task<Guid> Create(CreatePoolRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<CreatePoolRequestDto>(request);

		return await poolApiClient.Create(requestDto, cancellationToken);
	}

	public async Task<Guid> Update(UpdatePoolRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<UpdatePoolRequestDto>(request);

		return await poolApiClient.Update(requestDto, cancellationToken);
	}

	public async Task Delete(Guid id, CancellationToken cancellationToken)
	{
		await poolApiClient.Delete(id, cancellationToken);
	}
}
