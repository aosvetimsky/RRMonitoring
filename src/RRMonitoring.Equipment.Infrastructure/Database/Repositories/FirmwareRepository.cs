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
using RRMonitoring.Equipment.Domain.Models.Firmware;

namespace RRMonitoring.Equipment.Infrastructure.Database.Repositories;

internal class FirmwareRepository(EquipmentContext context) : RepositoryBase<Firmware, Guid>(context), IFirmwareRepository
{
	public async Task<Firmware> GetByName(string name, CancellationToken cancellationToken)
	{
		return await EntitiesDbSet
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
	}

	public async Task<PagedList<Firmware>> Search(SearchFirmwareCriteria criteria, CancellationToken cancellationToken)
	{
		var query = EntitiesDbSet
			.Include(x => x.FirmwareEquipmentModels)
				.ThenInclude(x => x.EquipmentModel)
			.AsNoTracking();

		if (!string.IsNullOrEmpty(criteria.Keyword))
		{
			query = query.Where(x => EF.Functions.ILike(x.Name, $"%{criteria.Keyword}%") || EF.Functions.ILike(x.Version, $"%{criteria.Keyword}%"));
		}

		if (criteria.EquipmentModelIds is not null)
		{
			query = query.Where(x => x.FirmwareEquipmentModels.Any(m => criteria.EquipmentModelIds.Contains(m.EquipmentModelId)));
		}

		return await query.ToSearchResult(criteria.SortExpressions, criteria.Skip, criteria.Take, cancellationToken);
	}
}
