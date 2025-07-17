using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Mining.Application.Features.Pools.Create;
using RRMonitoring.Mining.Application.Features.Pools.Delete;
using RRMonitoring.Mining.Application.Features.Pools.Get;
using RRMonitoring.Mining.Application.Features.Pools.Search;
using RRMonitoring.Mining.Application.Features.Pools.Update;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Api.Controllers;

[Route("v{version:apiVersion}/pool")]
[ApiController]
[Authorize]
public class PoolController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<PoolByIdResponseDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetPoolByIdRequest { Data = id }, cancellationToken);
	}

	[HttpPost("search")]
	public Task<PagedList<SearchPoolsResponseDto>> Search([FromBody] SearchPoolsRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchPoolsRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPost]
	public Task<Guid> Create([FromBody] CreatePoolRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new CreatePoolRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut]
	public Task<Guid> Update([FromBody] UpdatePoolRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new UpdatePoolRequest { Data = requestDto }, cancellationToken);
	}

	[HttpDelete("{id:guid}")]
	public Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new DeletePoolRequest(id), cancellationToken);
	}
}
