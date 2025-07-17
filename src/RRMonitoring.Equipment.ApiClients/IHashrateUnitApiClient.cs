using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Refit;
using RRMonitoring.Equipment.PublicModels.HashrateUnits;

namespace RRMonitoring.Equipment.ApiClients;

public interface IHashrateUnitApiClient
{
	[Get("/v1/hashrate-unit")]
	Task<IReadOnlyList<HashrateUnitResponseDto>> GetAll(CancellationToken cancellationToken);
}
