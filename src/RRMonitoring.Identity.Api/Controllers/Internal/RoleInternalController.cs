using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.Roles.Create;
using RRMonitoring.Identity.Application.Features.Roles.Delete;
using RRMonitoring.Identity.Application.Features.Roles.GetByCode;
using RRMonitoring.Identity.Application.Features.Roles.GetById;
using RRMonitoring.Identity.Application.Features.Roles.Search;
using RRMonitoring.Identity.Application.Features.Roles.Update;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("role")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class RoleInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public RoleInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	public async Task<RoleResponse> GetById([FromRoute] Guid id)
	{
		return await _mediator.Send(new GetRoleByIdRequest(id));
	}

	[HttpPost("get-by-ids")]
	public async Task<List<RoleResponse>> GetByIds([FromBody] IEnumerable<Guid> ids)
	{
		return await _mediator.Send(new GetRolesByIdsRequest(ids));
	}

	[HttpGet("by-codes")]
	public async Task<List<RolesByCodesResponse>> GetByCodes([FromQuery] IEnumerable<string> codes)
	{
		return await _mediator.Send(new GetRolesByCodesRequest(codes));
	}

	[HttpPost("search")]
	public async Task<PagedList<SearchRolesResponseItem>> Search([FromBody] SearchRolesRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPost]
	public async Task<Guid> Create([FromBody] CreateRoleRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPut]
	public async Task Update([FromBody] UpdateRoleRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	public async Task Delete([FromRoute] Guid id)
	{
		await _mediator.Send(new DeleteRoleRequest(id));
	}
}
