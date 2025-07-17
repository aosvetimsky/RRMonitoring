using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Services.Equipment;
using RRMonitoring.Bff.Admin.Application.Authentication;
using RRMonitoring.Bff.Admin.Application.Services.Equipment.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/equipment")]
[ApiController]
[Authorize]
public class EquipmentController(IEquipmentService equipmentService) : ControllerBase
{
	[HttpPost("search-manufacturers")]
	[Permission(Permissions.EquipmentRead)]
	public Task<PagedList<SearchEquipmentManufacturersResponse>> SearchManufacturers([FromBody] SearchEquipmentManufacturersRequest request, CancellationToken cancellationToken)
	{
		return equipmentService.SearchManufacturers(request, cancellationToken);
	}
}
