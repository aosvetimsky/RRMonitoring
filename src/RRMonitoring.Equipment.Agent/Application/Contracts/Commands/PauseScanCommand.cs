using System;

namespace RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

public record PauseScanCommand(Guid ScanId);
