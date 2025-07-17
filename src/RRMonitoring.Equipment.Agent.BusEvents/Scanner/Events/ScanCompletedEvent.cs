using System;
using System.Collections.Generic;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

public record ScanCompletedEvent(Guid ScanId, IReadOnlyList<DeviceInfo> Devices);
