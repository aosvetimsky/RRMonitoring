using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomium.Core.Data.EntityFrameworkCore.Extensions;
using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using Nomium.Core.Models;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.Domain.Models.Manufacturer;

namespace RRMonitoring.Equipment.Infrastructure.Database.Repositories;

internal class ManufacturerRepository(EquipmentContext context) : RepositoryBase<Manufacturer, Guid>(context), IManufacturerRepository
{
	public async Task<Manufacturer> GetByName(string name, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
	}

	public async Task<PagedList<Manufacturer>> Search(SearchManufacturersCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(p => EF.Functions.ILike(p.Name, $"%{criteria.Keyword}%"));
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}