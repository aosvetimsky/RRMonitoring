using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Identity.Api.Security;
using RRMonitoring.Identity.Application.Features.Tenants.Create;
using RRMonitoring.Identity.Application.Features.Tenants.Delete;
using RRMonitoring.Identity.Application.Features.Tenants.GetById;
using RRMonitoring.Identity.Application.Features.Tenants.Search;
using RRMonitoring.Identity.Application.Features.Tenants.Update;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("tenant")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TenantController : ControllerBase
{
	private readonly IMediator _mediator;

	public TenantController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	[Permission(Permissions.TenantRead)]
	public Task<TenantResponse> GetById([FromRoute] Guid id)
	{
		return _mediator.Send(new GetTenantByIdRequest(id));
	}

	[HttpPost("search")]
	[Permission(Permissions.TenantRead)]
	public Task<PagedList<SearchTenantsResponseItem>> Search([FromBody] SearchTenantsRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPost]
	[Permission(Permissions.TenantManage)]
	public async Task<Guid> CreateTenant([FromBody] CreateTenantRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPut]
	[Permission(Permissions.TenantManage)]
	public async Task UpdateTenant([FromBody] UpdateTenantRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	[Permission(Permissions.TenantManage)]
	public async Task DeleteTenant([FromRoute] Guid id)
	{
		await _mediator.Send(new DeleteTenantRequest(id));
	}
}
