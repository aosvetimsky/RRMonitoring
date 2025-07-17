using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Authentication;
using RRMonitoring.Bff.Admin.Application.Services.Facilities;
using RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/facility")]
[ApiController]
[Authorize]
public class FacilityController(IFacilityService facilityService) : ControllerBase
{
	[HttpGet("{id:Guid}")]
	[Permission(Permissions.FacilityRead)]
	public Task<FacilityByIdResponse> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return facilityService.GetById(id, cancellationToken);
	}

	[HttpPost("search")]
	[Permission(Permissions.FacilityRead)]
	public Task<PagedList<SearchFacilitiesResponse>> Search([FromBody] SearchFacilitiesRequest request, CancellationToken cancellationToken)
	{
		return facilityService.Search(request, cancellationToken);
	}

	[HttpPost]
	[Permission(Permissions.FacilityManage)]
	public Task<Guid> Create([FromBody] CreateFacilityRequest request, CancellationToken cancellationToken)
	{
		return facilityService.Create(request, cancellationToken);
	}

	[HttpPut]
	[Permission(Permissions.FacilityManage)]
	public async Task Update([FromBody] UpdateFacilityRequest request, CancellationToken cancellationToken)
	{
		await facilityService.Update(request, cancellationToken);
	}

	[HttpPut("{id:guid}/archive")]
	[Permission(Permissions.FacilityManage)]
	public async Task Archive([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await facilityService.Archive(id, cancellationToken);
	}

	[HttpPut("{id:guid}/unarchive")]
	[Permission(Permissions.FacilityManage)]
	public async Task Unarchive([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await facilityService.Unarchive(id, cancellationToken);
	}

	[HttpDelete("{id:Guid}")]
	[Permission(Permissions.FacilityManage)]
	public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await facilityService.Delete(id, cancellationToken);
	}
}
