using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Mining.Application.Features.Workers.Search;
using RRMonitoring.Mining.PublicModels.Workers;

namespace RRMonitoring.Mining.Api.Controllers;

[Route("v{version:apiVersion}/worker")]
[ApiController]
[Authorize]
public class WorkerController(IMediator mediator) : ControllerBase
{
	[HttpPost("search")]
	public Task<PagedList<SearchWorkersResponseDto>> Search([FromBody] SearchWorkersRequestDto requestDto, CancellationToken cancellationToken)
	{
		return mediator.Send(new SearchWorkersRequest { Data = requestDto }, cancellationToken);
	}
}
