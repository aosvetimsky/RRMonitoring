using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Authentication;
using RRMonitoring.Bff.Admin.Application.Services.Firmware;
using RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/firmware")]
[ApiController]
[Authorize]
public class FirmwareController(IFirmwareService firmwareService) : ControllerBase
{
	[HttpGet("{id:Guid}")]
	[Permission(Permissions.FirmwareRead)]
	public Task<FirmwareByIdResponse> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return firmwareService.GetById(id, cancellationToken);
	}

	[HttpGet("{id:guid}/file")]
	[Permission(Permissions.FirmwareRead)]
	public async Task<FileStreamResult> GetFile([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var response = await firmwareService.GetFile(id, cancellationToken);

		var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
		var fileName = response.Content.Headers.ContentDisposition?.FileName;

		return File(stream, "application/octet-stream", fileName);
	}

	[HttpPost("search")]
	[Permission(Permissions.FirmwareRead)]
	public Task<PagedList<SearchFirmwareResponse>> Search([FromBody] SearchFirmwareRequest request, CancellationToken cancellationToken)
	{
		return firmwareService.Search(request, cancellationToken);
	}

	[HttpPost]
	[Permission(Permissions.FirmwareManage)]
	public Task<Guid> Create([FromForm] CreateFirmwareRequest request, CancellationToken cancellationToken)
	{
		return firmwareService.Create(request, cancellationToken);
	}

	[HttpPut]
	[Permission(Permissions.FirmwareManage)]
	public async Task Update([FromBody] UpdateFirmwareRequest request, CancellationToken cancellationToken)
	{
		await firmwareService.Update(request, cancellationToken);
	}

	[HttpDelete("{id:Guid}")]
	[Permission(Permissions.FirmwareManage)]
	public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await firmwareService.Delete(id, cancellationToken);
	}
}
