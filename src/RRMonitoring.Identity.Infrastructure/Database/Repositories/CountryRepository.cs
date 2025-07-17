using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.Repositories;

public class CountryRepository : RepositoryBase<Country, int>, ICountryRepository
{
	private readonly DbSet<Country> _countriesDbSet;

	public CountryRepository(
		IdentityContext identityContext,
		IMemoryCache memoryCache)
		: base(identityContext)
	{
		_countriesDbSet = identityContext.Countries;
	}

	public async Task<List<Country>> GetActive(CancellationToken cancellationToken)
	{
		return await _countriesDbSet
			.AsNoTracking()
			.Where(country => country.IsActive)
			.OrderBy(country => country.Name)
			.ToListAsync(cancellationToken);
	}
}
