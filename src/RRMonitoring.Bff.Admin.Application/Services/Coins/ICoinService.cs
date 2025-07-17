using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Bff.Admin.Application.Services.Coins.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Coins;

public interface ICoinService
{
	Task<IReadOnlyList<CoinResponse>> GetAll(CancellationToken cancellationToken);
}
