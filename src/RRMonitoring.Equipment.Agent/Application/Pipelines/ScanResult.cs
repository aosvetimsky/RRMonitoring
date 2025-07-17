using System.Collections.Generic;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public record ScanResult(IReadOnlyList<DeviceInfo> FoundDevices);
