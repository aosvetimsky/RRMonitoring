using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Bff.Admin.Application.Services.Coins.Models;
using RRMonitoring.Bff.Admin.Application.Services.Coins;
using Microsoft.AspNetCore.Authorization;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/coin")]
[ApiController]
[Authorize]
public class CoinController(ICoinService coinService) : ControllerBase
{
	[HttpGet]
	public Task<IReadOnlyList<CoinResponse>> GetAll(CancellationToken cancellationToken)
	{
		return coinService.GetAll(cancellationToken);
	}
}
