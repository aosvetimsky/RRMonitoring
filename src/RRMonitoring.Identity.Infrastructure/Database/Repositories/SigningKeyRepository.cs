using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

internal class SigningKeyRepository : RepositoryBase<SigningKey, Guid>, ISigningKeyRepository
{
	private readonly DbSet<SigningKey> _signingKeys;

	public SigningKeyRepository(IdentityContext context) : base(context)
	{
		_signingKeys = context.Set<SigningKey>();
	}

	public Task<List<SigningKey>> Find(Expression<Func<SigningKey, bool>> predicate)
	{
		return _signingKeys.Where(predicate).ToListAsync();
	}
}
