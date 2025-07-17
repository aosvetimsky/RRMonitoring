using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Pools;
using RRMonitoring.Bff.Admin.Application.Services.Pools.Models;
using RRMonitoring.Bff.Admin.Application.Authentication;
using Nomium.Core.Security.Attributes;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/pool")]
[ApiController]
[Authorize]
public class PoolController(IPoolService poolService) : ControllerBase
{
	[HttpGet("{id:Guid}")]
	[Permission(Permissions.PoolRead)]
	public Task<PoolByIdResponse> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return poolService.GetById(id, cancellationToken);
	}

	[HttpPost("search")]
	[Permission(Permissions.PoolRead)]
	public Task<PagedList<SearchPoolsResponseItem>> Search([FromBody] SearchPoolsRequest request, CancellationToken cancellationToken)
	{
		return poolService.Search(request, cancellationToken);
	}

	[HttpPost]
	[Permission(Permissions.PoolManage)]
	public Task<Guid> Create([FromBody] CreatePoolRequest request, CancellationToken cancellationToken)
	{
		return poolService.Create(request, cancellationToken);
	}

	[HttpPut]
	[Permission(Permissions.PoolManage)]
	public Task<Guid> Update([FromBody] UpdatePoolRequest request, CancellationToken cancellationToken)
	{
		return poolService.Update(request, cancellationToken);
	}

	[HttpDelete("{id:Guid}")]
	[Permission(Permissions.PoolManage)]
	public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await poolService.Delete(id, cancellationToken);
	}
}
