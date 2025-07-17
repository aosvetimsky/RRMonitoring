using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.EquipmentModels.Models;
using RRMonitoring.Equipment.ApiClients;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;
using RRMonitoring.Mining.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Services.EquipmentModels;

public class EquipmentModelService(
	IEquipmentModelApiClient equipmentModelApiClient,
	ICoinApiClient coinApiClient,
	IMapper mapper) : IEquipmentModelService
{
	public async Task<EquipmentModelByIdResponse> GetById(Guid id, CancellationToken cancellationToken)
	{
		var equipmentModel = await equipmentModelApiClient.GetById(id, cancellationToken);

		return mapper.Map<EquipmentModelByIdResponse>(equipmentModel);
	}

	public async Task<PagedList<SearchEquipmentModelsResponse>> Search(SearchEquipmentModelsRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchEquipmentModelsRequestDto>(request);

		if (request.SortExpressions?.Any() == false)
		{
			requestDto.SortExpressions = new List<SortExpression>
			{
				new SortExpression { PropertyName = "CreatedDate", Direction = SortDirection.Desc }
			};
		}

		var equipmentModels = await equipmentModelApiClient.Search(requestDto, cancellationToken);

		var coinIds = equipmentModels.Items
			.SelectMany(x => x.CoinIds)
			.Distinct()
			.ToList();

		var coins = await coinApiClient.GetByIds(coinIds, cancellationToken);
		var coinsDictionary = coins.ToDictionary(x => x.Id, x => (x.Name, x.Ticker));

		return new PagedList<SearchEquipmentModelsResponse>
		{
			TotalCount = coins.Count,
			Items = equipmentModels.Items
				.Select(x => new SearchEquipmentModelsResponse
				{
					Id = x.Id,
					Name = x.Name,
					ManufacturerName = x.ManufacturerName,
					Hashrate = $"{x.NominalHashrate} {x.HashrateUnitName}",
					NominalPower = x.NominalPower,
					Coins = x.CoinIds
								.Select(x => new SearchEquipmentModelCoinsResponse
								{
									Id = x,
									Name = coinsDictionary.GetValueOrDefault(x).Name,
									Ticker = coinsDictionary.GetValueOrDefault(x).Ticker
								})
								.ToList()
				})
				.ToList()
		};
	}

	public async Task<Guid> Create(CreateEquipmentModelRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<CreateEquipmentModelRequestDto>(request);

		var coins = await coinApiClient.GetByIds(requestDto.CoinIds, cancellationToken);

		var existingCoinIds = coins
			.Select(x => x.Id)
			.ToArray();

		var nonExistingCoins = requestDto.CoinIds.Except(existingCoinIds).ToList();

		if (nonExistingCoins.Any())
		{
			throw new ValidationException($"Coins with ids \"{string.Join(',', nonExistingCoins)}\" don't exist.");
		}

		return await equipmentModelApiClient.Create(requestDto, cancellationToken);
	}

	public async Task<Guid> Update(UpdateEquipmentModelRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<UpdateEquipmentModelRequestDto>(request);

		var coins = await coinApiClient.GetByIds(requestDto.CoinIds, cancellationToken);

		var existingCoinIds = coins
			.Select(x => x.Id)
			.ToArray();

		var nonExistingCoins = requestDto.CoinIds.Except(existingCoinIds).ToList();

		if (nonExistingCoins.Any())
		{
			throw new ValidationException($"Coins with ids \"{string.Join(',', nonExistingCoins)}\" don't exist.");
		}

		return await equipmentModelApiClient.Update(requestDto, cancellationToken);
	}

	public async Task Delete(Guid id, CancellationToken cancellationToken)
	{
		await equipmentModelApiClient.Delete(id, cancellationToken);
	}
}
