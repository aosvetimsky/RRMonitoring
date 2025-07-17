using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Identity.Application.Features.Countries.GetActive;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("country")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CountryController : ControllerBase
{
	private readonly IMediator _mediator;

	public CountryController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public Task<List<CountryResponse>> GetActive()
	{
		return _mediator.Send(new GetActiveCountriesRequest());
	}
}
