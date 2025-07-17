using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using RRMonitoring.Notification.Domain.Contracts;

namespace RRMonitoring.Notification.Application.Features.DeviceRegistration.Unregister;

public class UnregisterDeviceRequest : IRequest
{
	public string RecipientId { get; set; }
	public string DeviceId { get; set; }
}

public class UnregisterDeviceHandler(
	IPushRegisteredDeviceRepository pushRegisteredDeviceRepository,
	ILogger<UnregisterDeviceRequest> logger)
	: IRequestHandler<UnregisterDeviceRequest>
{
	public async Task<Unit> Handle(UnregisterDeviceRequest request, CancellationToken cancellationToken)
	{
		var registeredDevice = await pushRegisteredDeviceRepository.Get(request.RecipientId, request.DeviceId);

		if (registeredDevice == null)
		{
			return Unit.Value;
		}

		await pushRegisteredDeviceRepository.Remove(registeredDevice);

		logger.LogInformation("Recipient with ID: {RecipientId} and DeviceID: {DeviceId} was unregistered",
			request.RecipientId, request.DeviceId);

		return Unit.Value;
	}
}
