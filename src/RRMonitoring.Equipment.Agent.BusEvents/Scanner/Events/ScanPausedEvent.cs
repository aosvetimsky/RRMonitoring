using System;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

public record ScanPausedEvent(Guid ScanId, DateTimeOffset PausedAt);