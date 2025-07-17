using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IExternalSourceRepository : IDictionaryRepository<ExternalSource, byte>
{
	Task<ExternalSource> GetByCode(string code, CancellationToken cancellationToken);
}
