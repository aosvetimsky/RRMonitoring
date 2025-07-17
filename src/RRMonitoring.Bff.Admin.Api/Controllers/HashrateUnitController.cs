using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/hashrate-unit")]
[ApiController]
[Authorize]
public class HashrateUnitController(IHashrateUnitService hashrateUnitService) : ControllerBase
{
	[HttpGet]
	public Task<IReadOnlyList<HashrateUnitResponse>> GetAll(CancellationToken cancellationToken)
	{
		return hashrateUnitService.GetAll(cancellationToken);
	}
}
