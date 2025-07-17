using System;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

public record ScanResumedEvent(Guid ScanId, DateTimeOffset ResumedAt);