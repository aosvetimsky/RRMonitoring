using System;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Application.Features.EquipmentModels.Search;
using RRMonitoring.Equipment.Application.Features.EquipmentModels.Get;
using RRMonitoring.Equipment.Application.Features.EquipmentModels.Create;
using RRMonitoring.Equipment.Application.Features.EquipmentModels.Update;
using RRMonitoring.Equipment.Application.Features.EquipmentModels.Delete;

namespace RRMonitoring.Equipment.Api.Controllers;

[Route("v{version:apiVersion}/equipment-model")]
[ApiController]
[Authorize]
public class EquipmentModelController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<EquipmentModelByIdResponseDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetEquipmentModelByIdRequest { Data = id }, cancellationToken);
	}

	[HttpPost("search")]
	public Task<PagedList<SearchEquipmentModelsResponseDto>> Search([FromBody] SearchEquipmentModelsRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchEquipmentModelsRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPost]
	public Task<Guid> Create([FromBody] CreateEquipmentModelRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new CreateEquipmentModelRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut]
	public Task<Guid> Update([FromBody] UpdateEquipmentModelRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new UpdateEquipmentModelRequest { Data = requestDto }, cancellationToken);
	}

	[HttpDelete("{id:guid}")]
	public Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new DeleteEquipmentModelRequest(id), cancellationToken);
	}
}
