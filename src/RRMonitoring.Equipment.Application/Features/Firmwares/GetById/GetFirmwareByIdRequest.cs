using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.PublicModels.Firmware;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.GetById;

public class GetFirmwareByIdRequest : BaseRequest<Guid, FirmwareByIdResponseDto>;

public class GetFirmwareByIdHandler(
	IFirmwareRepository firmwareRepository,
	IMapper mapper)
	: BaseRequestHandler<GetFirmwareByIdRequest, Guid, FirmwareByIdResponseDto>
{
	protected override async Task<FirmwareByIdResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var includePaths = new[]
		{
			nameof(Domain.Entities.Firmware.FirmwareEquipmentModels)
		};

		var firmware = await firmwareRepository.GetById(requestData, includePaths: includePaths, asNoTracking: true, cancellationToken: cancellationToken);

		if (firmware is null)
		{
			throw new ValidationException($"{nameof(Domain.Entities.Firmware)} with id {requestData} is not found");
		}

		return mapper.Map<FirmwareByIdResponseDto>(firmware);
	}
}
