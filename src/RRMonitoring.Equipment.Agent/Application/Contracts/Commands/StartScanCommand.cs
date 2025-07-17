using System;

namespace RRMonitoring.Equipment.Agent.Application.Contracts.Commands;

public record StartScanCommand(
	Guid ScanId,
	string Name,
	Guid SiteId,
	Guid? ContainerId,
	string IpMask,
	int TotalLimit,
	int BatchSize,
	DateTimeOffset RequestedAt);
