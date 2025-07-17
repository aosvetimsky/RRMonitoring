using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Delete;

public record DeleteManufacturerRequest(Guid Id) : IRequest;

public class DeleteManufacturerHandler(IManufacturerRepository manufacturerRepository) : IRequestHandler<DeleteManufacturerRequest>
{
	public async Task Handle(DeleteManufacturerRequest request, CancellationToken cancellationToken)
	{
		var manufacturer = await manufacturerRepository.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);

		if (manufacturer is null)
		{
			throw new ValidationException($"{nameof(Manufacturer)} with id: {request.Id} is not found.");
		}

		await manufacturerRepository.Delete(manufacturer, cancellationToken);
	}
}
