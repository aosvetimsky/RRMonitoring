using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Entities;
using RRMonitoring.Colocation.Domain.Models.Facility;

namespace RRMonitoring.Colocation.Infrastructure.Database.Repositories;

public class FacilityRepository(ColocationContext context) : RepositoryBase<Facility, Guid>(context), IFacilityRepository
{
	public async Task<Facility> GetByName(string name, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
	}

	public async Task<PagedList<Facility>> Search(SearchFacilitiesCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.Include(x => x.Technicians)
			.AsNoTracking();

		if (criteria.IsArchived.HasValue)
		{
			query = query.Where(x => x.IsArchived == criteria.IsArchived.Value);
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}
