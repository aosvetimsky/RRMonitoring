using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Delete;

public record DeleteFacilityRequest(Guid Id) : IRequest;

public class DeleteFacilityHandler(IFacilityRepository facilityRepository) : IRequestHandler<DeleteFacilityRequest>
{
	public async Task Handle(DeleteFacilityRequest request, CancellationToken cancellationToken)
	{
		var facility = await facilityRepository.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);

		if (facility is null)
		{
			throw new ValidationException($"{nameof(Facility)} with id: {request.Id} is not found.");
		}

		await facilityRepository.Delete(facility, cancellationToken);
	}
}
