using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels;
using RRMonitoring.Bff.Admin.Application.Authentication;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/equipment-model")]
[ApiController]
[Authorize]
public class EquipmentModelController(IEquipmentModelService equipmentModelService) : ControllerBase
{
	[HttpGet("{id:Guid}")]
	[Permission(Permissions.EquipmentModelManage)]
	public Task<EquipmentModelByIdResponse> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return equipmentModelService.GetById(id, cancellationToken);
	}

	[HttpPost("search")]
	[Permission(Permissions.EquipmentModelRead)]
	public Task<PagedList<SearchEquipmentModelsResponse>> Search([FromBody] SearchEquipmentModelsRequest request, CancellationToken cancellationToken)
	{
		return equipmentModelService.Search(request, cancellationToken);
	}

	[HttpPost]
	[Permission(Permissions.EquipmentModelManage)]
	public Task<Guid> Create([FromBody] CreateEquipmentModelRequest request, CancellationToken cancellationToken)
	{
		return equipmentModelService.Create(request, cancellationToken);
	}

	[HttpPut]
	[Permission(Permissions.EquipmentModelManage)]
	public Task<Guid> Update([FromBody] UpdateEquipmentModelRequest request, CancellationToken cancellationToken)
	{
		return equipmentModelService.Update(request, cancellationToken);
	}

	[HttpDelete("{id:Guid}")]
	[Permission(Permissions.EquipmentModelManage)]
	public async Task Delete([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		await equipmentModelService.Delete(id, cancellationToken);
	}
}
