using System;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

public record ScanCancelledEvent(Guid ScanId, DateTimeOffset CancelledAt);