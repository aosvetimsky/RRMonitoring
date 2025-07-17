using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.Data.Transactions;
using Nomium.Core.FileStorage.Providers.S3;
using Nomium.Core.FileStorage.Providers.S3.Models;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.Create;

public class CreateFirmwareRequest : BaseRequest<CreateFirmwareRequestDto, Guid>;

public class CreateFirmwareRequestHandler(
	IFirmwareRepository firmwareRepository,
	IEquipmentModelRepository equipmentModelRepository,
	IS3FileProvider s3FileProvider,
	FirmwareUrlResolver firmwareUrlResolver,
	ITransactionScopeManager transactionScopeManager)
	: BaseRequestHandler<CreateFirmwareRequest, CreateFirmwareRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(CreateFirmwareRequestDto requestData, CancellationToken cancellationToken)
	{
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

		if (firmwareWithTheSameName is not null)
		{
			throw new ValidationException($"{nameof(Firmware)} with name \"{requestData.Name}\" already exists.");
		}

		var firmwareToAdd = new Firmware
		{
			Name = requestData.Name,
			Version = requestData.Version,
			Comment = requestData.Comment,
			OriginFileName = requestData.File.FileName,
			FirmwareEquipmentModels = requestData.EquipmentModelIds
				.Select(x => new FirmwareEquipmentModel
				{
					EquipmentModelId = x
				})
				.ToArray()
		};

		await transactionScopeManager.Execute(async () =>
		{
			await firmwareRepository.Add(firmwareToAdd, cancellationToken);

			var filePath = firmwareUrlResolver.GetFilePathById(firmwareToAdd.Id, requestData.File.FileName);

			await using var stream = requestData.File.OpenReadStream();
			var uploadFileModel = new UploadFileModel
			{
				FilePath = filePath,
				Stream = stream
			};

			await s3FileProvider.UploadFile(uploadFileModel, cancellationToken);
		}, cancellationToken);

		return firmwareToAdd.Id;
	}
}
