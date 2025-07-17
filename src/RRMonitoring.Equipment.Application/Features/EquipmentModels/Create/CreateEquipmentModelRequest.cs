using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Create;

public class CreateEquipmentModelRequest : BaseRequest<CreateEquipmentModelRequestDto, Guid>;

public class CreateEquipmentModelRequestHandler(IEquipmentModelRepository equipmentModelRepository, IHashrateUnitRepository hashrateUnitRepository) : BaseRequestHandler<CreateEquipmentModelRequest, CreateEquipmentModelRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(CreateEquipmentModelRequestDto requestData, CancellationToken cancellationToken)
	{
		var hashrateUnit = await hashrateUnitRepository.GetById(requestData.HashrateUnitId, asNoTracking: true, cancellationToken: cancellationToken);

		if (hashrateUnit is null)
		{
			throw new ValidationException($"{nameof(HashrateUnit)} with id \"{requestData.HashrateUnitId}\" doesn't exist.");
		}

		var equipmentModelWithTheSameName = await equipmentModelRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (equipmentModelWithTheSameName is not null)
		{
			throw new ValidationException($"{nameof(EquipmentModel)} with name \"{requestData.Name}\" already exists.");
		}

		var equipmentModelToAdd = new EquipmentModel
		{
			Name = requestData.Name,
			ManufacturerId = requestData.ManufacturerId,
			HashrateUnitId = requestData.HashrateUnitId,
			NominalHashrate = requestData.NominalHashrate,
			NominalPower = requestData.NominalPower,
			MaxProcessorTemperature = requestData.MaxProcessorTemperature,
			MaxMotherBoardTemperature = requestData.MaxMotherBoardTemperature,
			EquipmentModelCoins = requestData.CoinIds
				.Select(c => new EquipmentModelCoin
				{
					CoinId = c
				})
				.ToList()
		};

		await equipmentModelRepository.Add(equipmentModelToAdd, cancellationToken);

		return equipmentModelToAdd.Id;
	}
}
