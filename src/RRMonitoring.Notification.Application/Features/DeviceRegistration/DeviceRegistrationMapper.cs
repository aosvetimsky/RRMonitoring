using AutoMapper;
using RRMonitoring.Notification.Application.Features.DeviceRegistration.Register;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Application.Features.DeviceRegistration;

public class DeviceRegistrationMapper : Profile
{
	public DeviceRegistrationMapper()
	{
		CreateMap<RegisterDeviceRequest, PushRegisteredDevice>();
	}
}
