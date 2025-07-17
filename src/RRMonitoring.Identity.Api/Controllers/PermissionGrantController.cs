using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Create;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Delete;
using RRMonitoring.Identity.Application.Features.PermissionGrants.GetById;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Search;
using RRMonitoring.Identity.Application.Features.PermissionGrants.Update;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("permission-grant")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PermissionGrantController
{
	private readonly IMediator _mediator;

	public PermissionGrantController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	public Task<PermissionGrantResponse> GetById([FromRoute] Guid id)
	{
		return _mediator.Send(new GetPermissionGrantByIdRequest(id));
	}

	[HttpPost("search")]
	public Task<PagedList<SearchPermissionGrantsResponseItem>> Search([FromBody] SearchPermissionGrantsRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPost]
	public Task<Guid> Create([FromBody] CreatePermissionGrantRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPut]
	public async Task Update([FromBody] UpdatePermissionGrantRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	public async Task Delete([FromRoute] Guid id)
	{
		await _mediator.Send(new DeletePermissionGrantRequest(id));
	}
}
