using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

public class ExternalSourceRepository : DictionaryRepository<ExternalSource, byte>, IExternalSourceRepository
{
	public ExternalSourceRepository(IdentityContext context)
		: base(context)
	{
	}

	public Task<ExternalSource> GetByCode(string code, CancellationToken cancellationToken)
	{
		return EntitiesDbSet
			.AsNoTracking()
			.SingleOrDefaultAsync(x => x.Code == code, cancellationToken);
	}
}
