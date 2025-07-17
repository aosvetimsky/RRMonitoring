using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Application.Features.Manufacturers.Create;
using RRMonitoring.Equipment.Application.Features.Manufacturers.Delete;
using RRMonitoring.Equipment.Application.Features.Manufacturers.Get;
using RRMonitoring.Equipment.Application.Features.Manufacturers.Search;
using RRMonitoring.Equipment.Application.Features.Manufacturers.Update;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Api.Controllers;

[Route("v{version:apiVersion}/manufacturer")]
[ApiController]
[Authorize]
public class ManufacturerController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<ManufacturerByIdResponseDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetManufacturerByIdRequest { Data = id }, cancellationToken);
	}

	[HttpPost("search")]
	public Task<PagedList<SearchManufacturersResponseDto>> Search([FromBody] SearchManufacturersRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchManufacturersRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPost]
	public Task<Guid> Create([FromBody] CreateManufacturerRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new CreateManufacturerRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut]
	public Task<Guid> Update([FromBody] UpdateManufacturerRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new UpdateManufacturerRequest { Data = requestDto }, cancellationToken);
	}

	[HttpDelete("{id:guid}")]
	public Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new DeleteManufacturerRequest(id), cancellationToken);
	}
}