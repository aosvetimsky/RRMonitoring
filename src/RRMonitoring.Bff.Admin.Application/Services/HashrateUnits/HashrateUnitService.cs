using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits.Models;
using RRMonitoring.Equipment.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Services.HashrateUnits;

internal class HashrateUnitService(IHashrateUnitApiClient hashrateUnitApiClient, IMapper mapper) : IHashrateUnitService
{
	public async Task<IReadOnlyList<HashrateUnitResponse>> GetAll(CancellationToken cancellationToken)
	{
		var hashrateUnits = await hashrateUnitApiClient.GetAll(cancellationToken);

		return mapper.Map<IReadOnlyList<HashrateUnitResponse>>(hashrateUnits);
	}
}
