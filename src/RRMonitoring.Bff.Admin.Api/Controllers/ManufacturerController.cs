using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Authentication;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/manufacturer")]
[ApiController]
[Authorize]
public class ManufacturerController(IManufacturerService manufacturerService) : ControllerBase
{
	[HttpGet("{id:Guid}")]
	[Permission(Permissions.ManufacturerRead)]
	public Task<ManufacturerByIdResponse> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return manufacturerService.GetById(id, cancellationToken);
	}

	[HttpPost("search")]
	[Permission(Permissions.ManufacturerRead)]
	public Task<PagedList<SearchManufacturersResponse>> Search([FromBody] SearchManufacturersRequest request, CancellationToken cancellationToken)
	{
		return manufacturerService.Search(request, cancellationToken);
	}

	[HttpPost]
	[Permission(Permissions.ManufacturerManage)]
	public Task<Guid> Create([FromBody] CreateManufacturerRequest request, CancellationToken cancellationToken)
	{
		return manufacturerService.Create(request, cancellationToken);
	}

	[HttpPut]
	[Permission(Permissions.ManufacturerManage)]
	public Task<Guid> Update([FromBody] UpdateManufacturerRequest request, CancellationToken cancellationToken)
	{
		return manufacturerService.Update(request, cancellationToken);
	}

	[HttpDelete("{id:Guid}")]
	[Permission(Permissions.ManufacturerManage)]
	public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await manufacturerService.Delete(id, cancellationToken);
	}
}