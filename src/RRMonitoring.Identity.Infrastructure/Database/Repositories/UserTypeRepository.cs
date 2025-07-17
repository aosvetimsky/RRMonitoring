using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

public class UserTypeRepository : DictionaryRepository<UserType, byte>, IUserTypeRepository
{
	public UserTypeRepository(IdentityContext context) : base(context)
	{
	}
}
