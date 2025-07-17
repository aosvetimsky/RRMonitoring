using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Nomium.Core.Data.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Domain.Contracts.Repositories;

public interface ISigningKeyRepository : IRepository<SigningKey, Guid>
{
	Task<List<SigningKey>> Find(Expression<Func<SigningKey, bool>> predicate);
}
