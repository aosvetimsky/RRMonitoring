using System;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Equipment.PublicModels.Firmware;
using RRMonitoring.Equipment.Application.Features.Firmwares.GetById;
using RRMonitoring.Equipment.Application.Features.Firmwares.Search;
using RRMonitoring.Equipment.Application.Features.Firmwares.Create;
using RRMonitoring.Equipment.Application.Features.Firmwares.Update;
using RRMonitoring.Equipment.Application.Features.Firmwares.Delete;
using RRMonitoring.Equipment.Application.Features.Firmwares.GetFile;

namespace RRMonitoring.Equipment.Api.Controllers;

[Route("v{version:apiVersion}/firmware")]
[ApiController]
[Authorize]
public class FirmwareController(IMediator mediator) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public Task<FirmwareByIdResponseDto> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetFirmwareByIdRequest { Data = id }, cancellationToken);
	}

	[HttpGet("{id:guid}/file")]
	public async Task<FileStreamResult> GetFile([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var response = await mediator.Send(new GetFirmwareFileRequest { Data = id }, cancellationToken);

		return File(response.Stream, "application/octet-stream", fileDownloadName: response.OriginFileName);
	}

	[HttpPost("search")]
	public Task<PagedList<SearchFirmwareResponseDto>> Search([FromBody] SearchFirmwareRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchFirmwareRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPost]
	public Task<Guid> Create([FromForm] CreateFirmwareRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new CreateFirmwareRequest { Data = requestDto }, cancellationToken);
	}

	[HttpPut]
	public Task Update([FromBody] UpdateFirmwareRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new UpdateFirmwareRequest { Data = requestDto }, cancellationToken);
	}

	[HttpDelete("{id:guid}")]
	public Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return mediator.Send(new DeleteFirmwareRequest(id), cancellationToken);
	}
}
