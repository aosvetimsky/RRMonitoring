using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Application.Features.EquipmentModels.Delete;

public record DeleteEquipmentModelRequest(Guid Id) : IRequest;

public class DeleteEquipmentModelHandler(IEquipmentModelRepository equipmentModelRepository) : IRequestHandler<DeleteEquipmentModelRequest>
{
	public async Task Handle(DeleteEquipmentModelRequest request, CancellationToken cancellationToken)
	{
		var equipmentModel = await equipmentModelRepository.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);

		if (equipmentModel is null)
		{
			throw new ValidationException($"{nameof(EquipmentModel)} with id: {request.Id} is not found.");
		}

		await equipmentModelRepository.Delete(equipmentModel, cancellationToken);
	}
}
