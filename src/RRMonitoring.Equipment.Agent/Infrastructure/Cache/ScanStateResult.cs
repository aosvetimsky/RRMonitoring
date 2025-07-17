namespace RRMonitoring.Equipment.Agent.Infrastructure.Cache;

public readonly record struct ScanStateResult(bool Success, string HangfireJobId);
