using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Identity.Api.Security;
using RRMonitoring.Identity.Application.Features.Roles.Create;
using RRMonitoring.Identity.Application.Features.Roles.Delete;
using RRMonitoring.Identity.Application.Features.Roles.GetById;
using RRMonitoring.Identity.Application.Features.Roles.Search;
using RRMonitoring.Identity.Application.Features.Roles.Update;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("role")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class RoleController : ControllerBase
{
	private readonly IMediator _mediator;

	public RoleController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	[Permission(Permissions.RoleRead)]
	public async Task<RoleResponse> GetById([FromRoute] Guid id)
	{
		return await _mediator.Send(new GetRoleByIdRequest(id));
	}

	[HttpPost("get-by-ids")]
	[Permission(Permissions.RoleRead)]
	public async Task<List<RoleResponse>> GetByIds([FromBody] IEnumerable<Guid> ids)
	{
		return await _mediator.Send(new GetRolesByIdsRequest(ids));
	}

	[HttpPost("search")]
	[Permission(Permissions.RoleRead)]
	public async Task<PagedList<SearchRolesResponseItem>> Search([FromBody] SearchRolesRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPost]
	[Permission(Permissions.RoleManage)]
	public async Task<Guid> Create([FromBody] CreateRoleRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPut]
	[Permission(Permissions.RoleManage)]
	public async Task Update([FromBody] UpdateRoleRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	[Permission(Permissions.RoleManage)]
	public async Task Delete([FromRoute] Guid id)
	{
		await _mediator.Send(new DeleteRoleRequest(id));
	}
}
