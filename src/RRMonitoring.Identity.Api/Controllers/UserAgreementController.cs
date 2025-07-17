using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.Auth.Agreement;

namespace RRMonitoring.Identity.Api.Controllers;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class UserAgreementController : Controller
{
	private readonly IMediator _mediator;

	public UserAgreementController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("accept-agreement", Name = "AcceptAgreement")]
	public async Task<IActionResult> AcceptAsync([FromQuery] string returnUrl)
	{
		var result = await _mediator.Send(new AcceptAgreementRequest());
		if (result.IsSuccess)
		{
			return Redirect(returnUrl);
		}

		return LocalRedirect("/identity/error");
	}

	[HttpPost("reject-agreement", Name = "RejectAgreement")]
	public IActionResult RejectAsync([FromQuery] string returnUrl)
	{
		return RedirectToAction("Login", "Auth", new { ReturnUrl = returnUrl });
	}
}
