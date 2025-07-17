using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Bff.Admin.Application.Services.HashrateUnits.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.HashrateUnits;

public interface IHashrateUnitService
{
	Task<IReadOnlyList<HashrateUnitResponse>> GetAll(CancellationToken cancellationToken);
}
