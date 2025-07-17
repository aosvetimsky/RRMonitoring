using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Update;

public class UpdateEquipmentModelRequest : BaseRequest<UpdateEquipmentModelRequestDto, Guid>;

public class UpdateEquipmentModelRequestHandler(IEquipmentModelRepository equipmentModelRepository, IHashrateUnitRepository hashrateUnitRepository) :
	BaseRequestHandler<UpdateEquipmentModelRequest, UpdateEquipmentModelRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(UpdateEquipmentModelRequestDto requestData, CancellationToken cancellationToken)
	{
		var equipmentModel = await equipmentModelRepository.GetById(requestData.Id, includePaths: [nameof(EquipmentModel.EquipmentModelCoins)], cancellationToken: cancellationToken);

		if (equipmentModel is null)
		{
			throw new ValidationException($"{nameof(EquipmentModel)} with id: {requestData.Id} is not found.");
		}

		var hashrateUnit = await hashrateUnitRepository.GetById(requestData.HashrateUnitId, asNoTracking: true, cancellationToken: cancellationToken);

		if (hashrateUnit is null)
		{
			throw new ValidationException($"{nameof(HashrateUnit)} with id \"{requestData.HashrateUnitId}\" doesn't exist.");
		}

		var equipmentModelWithTheSameName = await equipmentModelRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (equipmentModelWithTheSameName is not null && equipmentModelWithTheSameName.Id != requestData.Id)
		{
			throw new ValidationException($"{nameof(EquipmentModel)} with name \"{requestData.Name}\" already exists.");
		}

		equipmentModel.Name = requestData.Name;
		equipmentModel.ManufacturerId = requestData.ManufacturerId;
		equipmentModel.HashrateUnitId = requestData.HashrateUnitId;
		equipmentModel.NominalHashrate = requestData.NominalHashrate;
		equipmentModel.NominalPower = requestData.NominalPower;
		equipmentModel.MaxMotherBoardTemperature = requestData.MaxMotherBoardTemperature;
		equipmentModel.MaxProcessorTemperature = requestData.MaxProcessorTemperature;
		equipmentModel.EquipmentModelCoins = requestData.CoinIds
			.Select(x => new EquipmentModelCoin
			{
				CoinId = x
			})
			.ToList();

		await equipmentModelRepository.Update(equipmentModel, cancellationToken);

		return equipmentModel.Id;
	}
}
