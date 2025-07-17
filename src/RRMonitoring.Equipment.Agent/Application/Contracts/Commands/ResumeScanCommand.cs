using System;

namespace RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

public record ResumeScanCommand(Guid ScanId);
