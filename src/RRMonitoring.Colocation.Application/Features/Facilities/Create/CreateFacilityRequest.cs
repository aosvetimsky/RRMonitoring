using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Entities;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Create;

public class CreateFacilityRequest : BaseRequest<CreateFacilityRequestDto, Guid>;

public class CreateFacilityRequestHandler(IFacilityRepository facilityRepository) : BaseRequestHandler<CreateFacilityRequest, CreateFacilityRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(CreateFacilityRequestDto requestData, CancellationToken cancellationToken)
	{
		var facilityWithTheSameName = await facilityRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (facilityWithTheSameName is not null)
		{
			throw new ValidationException($"{nameof(Facility)} with name \"{requestData.Name}\" already exists.");
		}

		var facilityToAdd = new Facility
		{
			Name = requestData.Name,
			ManagerId = requestData.ManagerId,
			DeputyManagerId = requestData.DeputyManagerId,
			PowerCapacity = requestData.PowerCapacity,
			Subnet = requestData.Subnet,
			Technicians = requestData.TechnicianIds
				.Select(x => new FacilityTechnician
				{
					UserId = x
				}).ToArray()
		};

		await facilityRepository.Add(facilityToAdd, cancellationToken);

		return facilityToAdd.Id;
	}
}
