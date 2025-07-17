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
using RRMonitoring.Equipment.Domain.Models.EquipmentModel;

namespace RRMonitoring.Equipment.Infrastructure.Database.Repositories;

internal class EquipmentModelRepository(EquipmentContext context) : RepositoryBase<EquipmentModel, Guid>(context), IEquipmentModelRepository
{
	public async Task<EquipmentModel> GetByName(string name, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
	}

	public async Task<PagedList<EquipmentModel>> Search(SearchEquipmentModelsCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.Include(x => x.EquipmentModelCoins)
			.Include(x => x.HashrateUnit)
			.Include(x => x.Manufacturer)
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(x => EF.Functions.ILike(x.Name, $"%{criteria.Keyword}%"));
		}

		if (criteria.ManufacturerIds != null)
		{
			query = query.Where(x => criteria.ManufacturerIds.Contains(x.ManufacturerId));
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}