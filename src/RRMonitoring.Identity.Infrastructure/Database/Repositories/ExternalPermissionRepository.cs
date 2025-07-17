using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

public class ExternalPermissionRepository : DictionaryRepository<ExternalPermission, byte>, IExternalPermissionRepository
{
	public ExternalPermissionRepository(IdentityContext context) : base(context)
	{
	}
}
