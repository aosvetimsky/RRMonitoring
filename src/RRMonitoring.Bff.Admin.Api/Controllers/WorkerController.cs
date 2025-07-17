using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Authentication;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Bff.Admin.Application.Services.Workers;
using RRMonitoring.Bff.Admin.Application.Services.Workers.Models;

namespace RRMonitoring.Bff.Admin.Api.Controllers;

[Route("v{version:apiVersion}/worker")]
[ApiController]
[Authorize]
public class WorkerController(IWorkerService workerService) : ControllerBase
{
	[HttpPost("search")]
	[Permission(Permissions.WorkerRead)]
	public Task<PagedList<SearchWorkersResponse>> Search([FromBody] SearchWorkersRequest request, CancellationToken cancellationToken)
	{
		return workerService.Search(request, cancellationToken);
	}
}
