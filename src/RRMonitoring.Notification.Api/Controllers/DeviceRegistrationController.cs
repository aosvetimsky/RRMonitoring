using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RRMonitoring.Notification.Application.Features.DeviceRegistration.Register;
using RRMonitoring.Notification.Application.Features.DeviceRegistration.Unregister;

namespace RRMonitoring.Notification.Api.Controllers;

[Route("device-registration")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceRegistrationController(IMediator mediator)
{
	[HttpPost("register")]
	public async Task Register(RegisterDeviceRequest request)
	{
		await mediator.Send(request);
	}

	[HttpPost("unregister")]
	public async Task Unregister(UnregisterDeviceRequest request)
	{
		await mediator.Send(request);
	}
}
