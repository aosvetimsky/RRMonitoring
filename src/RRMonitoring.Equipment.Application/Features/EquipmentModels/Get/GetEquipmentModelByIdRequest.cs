using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.EquipmentModels;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Get;

public class GetEquipmentModelByIdRequest : BaseRequest<Guid, EquipmentModelByIdResponseDto>;

public class GetEquipmentModelByIdHandler(IEquipmentModelRepository equipmentModelRepository, IMapper mapper) : BaseRequestHandler<GetEquipmentModelByIdRequest, Guid, EquipmentModelByIdResponseDto>
{
	protected override async Task<EquipmentModelByIdResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var includePaths = new[]
		{
			nameof(EquipmentModel.EquipmentModelCoins),
			nameof(EquipmentModel.HashrateUnit),
			nameof(EquipmentModel.Manufacturer)
		};

		var equipmentModel = await equipmentModelRepository.GetById(requestData, includePaths: includePaths, asNoTracking: true, cancellationToken: cancellationToken);

		if (equipmentModel is null)
		{
			throw new ValidationException($"{nameof(EquipmentModel)} with id {requestData} is not found");
		}

		return mapper.Map<EquipmentModelByIdResponseDto>(equipmentModel);
	}
}
