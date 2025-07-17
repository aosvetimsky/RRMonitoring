using System;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Scanner.Events;

public record ScanProgressEvent(Guid ScanId, int Total, int Checked, int Alive);
