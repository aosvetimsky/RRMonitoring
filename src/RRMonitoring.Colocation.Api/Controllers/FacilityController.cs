using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Colocation.Application.Features.Facilities.Archive;
using RRMonitoring.Colocation.Application.Features.Facilities.Create;
using RRMonitoring.Colocation.Application.Features.Facilities.Delete;
using RRMonitoring.Colocation.Application.Features.Facilities.GetById;
using RRMonitoring.Colocation.Application.Features.Facilities.Search;
using RRMonitoring.Colocation.Application.Features.Facilities.Unarchive;
using RRMonitoring.Colocation.Application.Features.Facilities.Update;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Api.Controllers;

[Route("v{version:apiVersion}/facility")]
[ApiController]
[Authorize]
public class FacilityController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<FacilityByIdResponseDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetFacilityByIdRequest { Data = id }, cancellationToken);
	}

	[HttpPost("search")]
	public Task<PagedList<SearchFacilitiesResponseDto>> Search([FromBody] SearchFacilitiesRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchFacilitiesRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPost]
	public Task<Guid> Create([FromBody] CreateFacilityRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new CreateFacilityRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut]
	public Task Update([FromBody] UpdateFacilityRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new UpdateFacilityRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut("{id:guid}/archive")]
	public Task Archive([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new ArchiveFacilityRequest { Data = id }, cancellationToken);
	}

	[HttpPut("{id:guid}/unarchive")]
	public Task Unarchive([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new UnarchiveFacilityRequest { Data = id }, cancellationToken);
	}

	[HttpDelete("{id:guid}")]
	public Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new DeleteFacilityRequest(id), cancellationToken);
	}
}
