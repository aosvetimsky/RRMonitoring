using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Update;

public class UpdateManufacturerRequest : BaseRequest<UpdateManufacturerRequestDto, Guid>;

public class UpdateManufacturerRequestHandler(IManufacturerRepository manufacturerRepository) : BaseRequestHandler<UpdateManufacturerRequest, UpdateManufacturerRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(UpdateManufacturerRequestDto requestData, CancellationToken cancellationToken)
	{
		var manufacturer = await manufacturerRepository.GetById(requestData.Id, cancellationToken: cancellationToken);

		if (manufacturer is null)
		{
			throw new ValidationException($"{nameof(Manufacturer)} with id: {requestData.Id} is not found.");
		}

		var existingManufacturer = await manufacturerRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (existingManufacturer is not null && existingManufacturer.Id != requestData.Id)
		{
			throw new ValidationException($"{nameof(Manufacturer)} with name \"{requestData.Name}\" already exists.");
		}

		manufacturer.Name = requestData.Name;

		await manufacturerRepository.Update(manufacturer, cancellationToken);

		return manufacturer.Id;
	}
}
