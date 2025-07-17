using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RRMonitoring.Notification.Domain.Contracts;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Application.Features.DeviceRegistration.Register;

public class RegisterDeviceRequest : IRequest
{
	public string RecipientId { get; set; }
	public string DeviceId { get; set; }
	public string Token { get; set; }
}

public class RegisterDeviceHandler(
	IPushRegisteredDeviceRepository pushRegisteredDeviceRepository,
	ILogger<RegisterDeviceRequest> logger,
	IMapper mapper)
	: IRequestHandler<RegisterDeviceRequest>
{
	public async Task<Unit> Handle(RegisterDeviceRequest request, CancellationToken cancellationToken)
	{
		var registeredDevice = await pushRegisteredDeviceRepository.Get(request.RecipientId, request.DeviceId);

		if (registeredDevice == null)
		{
			var newRegisteredDevice = mapper.Map<PushRegisteredDevice>(request);

			await pushRegisteredDeviceRepository.Add(newRegisteredDevice);
		}
		else
		{
			registeredDevice.Token = request.Token;

			await pushRegisteredDeviceRepository.Update(registeredDevice);
		}

		logger.LogInformation("Recipient with ID: {RecipientId} and DeviceID: {DeviceId} was registered",
			request.RecipientId, request.DeviceId);

		return Unit.Value;
	}
}
