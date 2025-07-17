using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.GetById;

public class GetFacilityByIdRequest : BaseRequest<Guid, FacilityByIdResponseDto>;

public class GetFacilityByIdHandler(IFacilityRepository facilityRepository, IMapper mapper) : BaseRequestHandler<GetFacilityByIdRequest, Guid, FacilityByIdResponseDto>
{
	protected override async Task<FacilityByIdResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var includePaths = new[]
		{
			nameof(Domain.Entities.Facility.Technicians)
		};

		var facility = await facilityRepository.GetById(requestData, includePaths: includePaths, asNoTracking: true, cancellationToken: cancellationToken);

		if (facility is null)
		{
			throw new ValidationException($"{nameof(Domain.Entities.Facility)} with id {requestData} is not found");
		}

		return mapper.Map<FacilityByIdResponseDto>(facility);
	}
}
