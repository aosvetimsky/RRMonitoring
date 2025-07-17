using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Colocation.Domain.Contracts.Repositories;
using RRMonitoring.Colocation.Domain.Entities;
using RRMonitoring.Colocation.PublicModels.Facilities;

namespace RRMonitoring.Colocation.Application.Features.Facilities.Update;

public class UpdateFacilityRequest : BaseRequest<UpdateFacilityRequestDto, Unit>;

public class UpdateFacilityRequestHandler(IFacilityRepository facilityRepository) : BaseRequestHandler<UpdateFacilityRequest, UpdateFacilityRequestDto, Unit>
{
	protected override async Task<Unit> HandleData(UpdateFacilityRequestDto requestData, CancellationToken cancellationToken)
	{
		var facility = await facilityRepository.GetById(requestData.Id, includePaths: [nameof(Facility.Technicians)], cancellationToken: cancellationToken);

		if (facility is null)
		{
			throw new ValidationException($"{nameof(Facility)} with id: {requestData.Id} is not found.");
		}

		var facilityWithTheSameName = await facilityRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);

		if (facilityWithTheSameName is not null && facility.Id != requestData.Id)
		{
			throw new ValidationException($"{nameof(Facility)} with name \"{requestData.Name}\" already exists.");
		}

		facility.Name = requestData.Name;
		facility.ManagerId = requestData.ManagerId;
		facility.DeputyManagerId = requestData.DeputyManagerId;
		facility.PowerCapacity = requestData.PowerCapacity;
		facility.Subnet = requestData.Subnet;
		facility.Technicians = requestData.TechnicianIds
			.Select(x => new FacilityTechnician
			{
				UserId = x
			})
			.ToList();

		await facilityRepository.Update(facility, cancellationToken);

		return Unit.Value;
	}
}
