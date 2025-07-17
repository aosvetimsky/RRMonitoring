using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.Update;

public class UpdateFirmwareRequest : BaseRequest<UpdateFirmwareRequestDto, Unit>;

public class UpdateFirmwareRequestHandler(
	IFirmwareRepository firmwareRepository,
	IEquipmentModelRepository equipmentModelRepository)
	: BaseRequestHandler<UpdateFirmwareRequest, UpdateFirmwareRequestDto, Unit>
{
	protected override async Task<Unit> HandleData(UpdateFirmwareRequestDto requestData, CancellationToken cancellationToken)
	{
		var firmware = await firmwareRepository.GetById(requestData.Id, includePaths: [nameof(Firmware.FirmwareEquipmentModels)], cancellationToken: cancellationToken);

		if (firmware is null)
		{
			throw new ValidationException($"{nameof(Firmware)} with id: {requestData.Id} is not found.");
		}

		var existingEquipmentModels = await equipmentModelRepository.GetByIds(requestData.EquipmentModelIds, asNoTracking: true, cancellationToken: cancellationToken);

		var existingEquipmentModelIds = existingEquipmentModels
			.Select(c => c.Id)
			.ToArray();

		var nonExistingEquipmentModelIds = requestData.EquipmentModelIds.Except(existingEquipmentModelIds).ToList();

		if (nonExistingEquipmentModelIds.Any())
		{
			throw new ValidationException($"Equipment Models with ids \"{string.Join(',', nonExistingEquipmentModelIds)}\" don't exist.");
		}

		var firmwareWithTheSameName = await firmwareRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (firmwareWithTheSameName is not null && firmware.Id != requestData.Id)
		{
			throw new ValidationException($"{nameof(Firmware)} with name \"{requestData.Name}\" already exists.");
		}

		firmware.Name = requestData.Name;
		firmware.Version = requestData.Version;
		firmware.Comment = requestData.Comment;
		firmware.FirmwareEquipmentModels = requestData.EquipmentModelIds
			.Select(x => new Domain.Entities.FirmwareEquipmentModel
			{
				EquipmentModelId = x
			})
			.ToArray();

		await firmwareRepository.Update(firmware, cancellationToken);

		return Unit.Value;
	}
}
