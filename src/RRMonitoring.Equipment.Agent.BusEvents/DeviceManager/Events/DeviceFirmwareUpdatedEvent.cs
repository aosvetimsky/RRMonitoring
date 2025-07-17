using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Scanner.BusEvents.DeviceManager.Events;

public record DeviceFirmwareUpdatedEvent(DeviceInfo Device);
