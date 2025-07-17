using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Equipment.Application.Features.HashrateUnits.Get;
using RRMonitoring.Equipment.PublicModels.HashrateUnits;

namespace RRMonitoring.Equipment.Api.Controllers;

[Route("v{version:apiVersion}/hashrate-unit")]
[ApiController]
[Authorize]
public class HashrateUnitController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public Task<IReadOnlyList<HashrateUnitResponseDto>> GetAll(CancellationToken cancellationToken)
	{
		return mediator.Send(new GetAllHashrateUnitsRequest(), cancellationToken);
	}
}
