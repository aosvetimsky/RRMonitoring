using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Archive;

public class ArchiveFacilityRequest : BaseRequest<Guid>;

public class ArchiveFacilityRequestHandler(IFacilityRepository facilityRepository) : BaseRequestHandler<ArchiveFacilityRequest, Guid>
{
	protected override async Task<Unit> HandleData(Guid id, CancellationToken cancellationToken)
	{
		var facility = await facilityRepository.GetById(id, cancellationToken: cancellationToken);

		if (facility is null)
		{
			throw new ValidationException($"{nameof(Facility)} with id: {id} is not found.");
		}

		if (facility.IsArchived)
		{
			throw new ValidationException($"{nameof(Facility)} with id: {id} is already archived.");
		}

		facility.IsArchived = true;

		await facilityRepository.Update(facility, cancellationToken);

		return Unit.Value;
	}
}
