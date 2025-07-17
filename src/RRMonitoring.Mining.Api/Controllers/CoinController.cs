using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Mining.Application.Features.Coins.Get;
using RRMonitoring.Mining.PublicModels.Coins;

namespace RRMonitoring.Mining.Api.Controllers;

[Route("v{version:apiVersion}/coin")]
[ApiController]
[Authorize]
public class CoinController(IMediator mediator) : ControllerBase
{
	[HttpGet("get-by-ids")]
	public Task<IReadOnlyList<CoinByIdResponseDto>> GetByIds([FromBody] IReadOnlyList<byte> ids, CancellationToken cancellationToken)
	{
		return mediator.Send(new GetCoinsByIdsRequest { Data = ids }, cancellationToken);
	}

	[HttpGet]
	public Task<IReadOnlyList<CoinResponseDto>> GetAll(CancellationToken cancellationToken)
	{
		return mediator.Send(new GetAllCoinsRequest(), cancellationToken);
	}
}
