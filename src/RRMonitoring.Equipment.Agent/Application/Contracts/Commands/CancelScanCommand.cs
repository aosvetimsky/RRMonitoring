using System;

namespace RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

public record CancelScanCommand(Guid ScanId);
