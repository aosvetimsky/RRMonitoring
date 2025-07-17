using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Create;

public class CreateManufacturerRequest : BaseRequest<CreateManufacturerRequestDto, Guid>;

public class CreateManufacturerRequestHandler(IManufacturerRepository manufacturerRepository) : BaseRequestHandler<CreateManufacturerRequest, CreateManufacturerRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(CreateManufacturerRequestDto requestData, CancellationToken cancellationToken)
	{
		var sameNameManufacturer = await manufacturerRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (sameNameManufacturer is not null)
		{
			throw new ValidationException($"{nameof(Manufacturer)} with name \"{requestData.Name}\" already exists.");
		}

		var manufacturerToAdd = new Manufacturer
		{
			Name = requestData.Name
		};

		await manufacturerRepository.Add(manufacturerToAdd, cancellationToken);

		return manufacturerToAdd.Id;
	}
}
