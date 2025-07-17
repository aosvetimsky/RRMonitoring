using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal class UserStatusRepository : DictionaryRepository<UserStatus, byte>, IUserStatusRepository
{
	public UserStatusRepository(IdentityContext context) : base(context)
	{
	}
}
