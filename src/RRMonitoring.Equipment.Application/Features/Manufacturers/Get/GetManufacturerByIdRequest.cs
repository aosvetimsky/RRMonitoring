using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Equipment.Application.Features.Manufacturers.Get;

public class GetManufacturerByIdRequest : BaseRequest<Guid, ManufacturerByIdResponseDto>;

public class GetManufacturerByIdHandler(IManufacturerRepository manufacturerRepository) : BaseRequestHandler<GetManufacturerByIdRequest, Guid, ManufacturerByIdResponseDto>
{
	protected override async Task<ManufacturerByIdResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var manufacturer = await manufacturerRepository.GetById(requestData, asNoTracking: true, cancellationToken: cancellationToken);

		if (manufacturer is null)
		{
			throw new ValidationException($"{nameof(Manufacturer)} with id {requestData} is not found");
		}

		return new ManufacturerByIdResponseDto
		{
			Id = manufacturer.Id,
			Name = manufacturer.Name
		};
	}
}
