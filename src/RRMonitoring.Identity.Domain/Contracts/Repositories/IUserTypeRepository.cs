using Nomium.Core.Data.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface IUserTypeRepository : IDictionaryRepository<UserType, byte>
{
}
