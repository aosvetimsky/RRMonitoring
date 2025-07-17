using System;
using System.Collections.Generic;
using RRMonitoring.Equipment.Scanner.BusEvents.Models;

namespace RRMonitoring.Equipment.Agent.Application.Pipelines;

public record ScanContext(StackExchange.Redis.IDatabase Redis, Guid ScanId, List<DeviceInfo> Buffer);
