using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Application.Features.Tenants.Create;
using RRMonitoring.Identity.Application.Features.Tenants.Delete;
using RRMonitoring.Identity.Application.Features.Tenants.GetByCode;
using RRMonitoring.Identity.Application.Features.Tenants.GetById;
using RRMonitoring.Identity.Application.Features.Tenants.Search;
using RRMonitoring.Identity.Application.Features.Tenants.Update;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("tenant")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class TenantInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public TenantInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	public Task<TenantResponse> GetById([FromRoute] Guid id)
	{
		return _mediator.Send(new GetTenantByIdRequest(id));
	}

	[HttpGet("by-code/{code}")]
	public Task<TenantByCodeResponse> GetByCode([FromRoute] string code)
	{
		return _mediator.Send(new GetTenantByCodeRequest { Code = code });
	}

	[HttpPost("search")]
	public Task<PagedList<SearchTenantsResponseItem>> Search([FromBody] SearchTenantsRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPost]
	public async Task<Guid> CreateTenant([FromBody] CreateTenantRequest request)
	{
		return await _mediator.Send(request);
	}

	[HttpPut]
	public async Task UpdateTenant([FromBody] UpdateTenantRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpDelete("{id:guid}")]
	public async Task DeleteTenant([FromRoute] Guid id)
	{
		await _mediator.Send(new DeleteTenantRequest(id));
	}
}
