using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.FileStorage.Providers.S3;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.GetFile;

public class GetFirmwareFileRequest : BaseRequest<Guid, FileResponseDto>;

public class GetFirmwareFileRequestHandler(
	IFirmwareRepository firmwareRepository,
	IS3FileProvider s3FileProvider,
	FirmwareUrlResolver firmwareUrlResolver)
	: BaseRequestHandler<GetFirmwareFileRequest, Guid, FileResponseDto>
{
	protected override async Task<FileResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var firmware = await firmwareRepository.GetById(requestData, asNoTracking: true, cancellationToken: cancellationToken);

		if (firmware is null)
		{
			throw new ValidationException($"{nameof(Firmware)} with id {requestData} is not found");
		}

		var stream = await s3FileProvider.GetFileAsStream(firmwareUrlResolver.GetFilePathById(firmware.Id, firmware.OriginFileName), cancellationToken: cancellationToken);

		return new FileResponseDto
		{
			Stream = stream,
			OriginFileName = firmware.OriginFileName
		};
	}
}
