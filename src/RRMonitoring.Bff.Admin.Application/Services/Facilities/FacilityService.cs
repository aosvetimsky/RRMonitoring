using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Exceptions;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;
using RRMonitoring.Colocation.ApiClients;
using RRMonitoring.Colocation.PublicModels.Facilities;
using RRMonitoring.Identity.ApiClients.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities;

public class FacilityService(
	IFacilityApiClient facilityApiClient,
	IUserInternalApiClient userInternalApiClient,
	IMapper mapper)
	: IFacilityService
{
	public async Task<FacilityByIdResponse> GetById(Guid id, CancellationToken cancellationToken)
	{
		var facility = await facilityApiClient.GetById(id, cancellationToken);

		return mapper.Map<FacilityByIdResponse>(facility);
	}

	public async Task<PagedList<SearchFacilitiesResponse>> Search(SearchFacilitiesRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchFacilitiesRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "Name", Direction = SortDirection.Asc }
		};

		var facilities = await facilityApiClient.Search(requestDto, cancellationToken);

		var managerIds = facilities.Items
			.Select(x => x.ManagerId)
			.ToArray();
		var deputyManagerIds = facilities.Items
			.Select(x => x.DeputyManagerId)
			.ToArray();
		var techniciansIds = facilities.Items
			.SelectMany(x => x.TechnicianIds)
			.ToArray();

		var userIds = managerIds
			.Union(deputyManagerIds)
			.Union(techniciansIds)
			.Distinct();

		var users = await userInternalApiClient.GetByIds(userIds, cancellationToken);
		var usersDictionary = users.ToDictionary(x => x.Id, x => $"{x.FirstName} {x.LastName}");

		return new PagedList<SearchFacilitiesResponse>
		{
			TotalCount = usersDictionary.Count,
			Items = facilities.Items
				.Select(x => new SearchFacilitiesResponse
				{
					Id = x.Id,
					Name = x.Name,
					PowerCapacity = x.PowerCapacity,
					PowerReserve = 0, // TODO
					EquipmentQuantity = 0, // TODO fill from Equipment service when it's done there
					EquipmentUnderRepairQuantity = 0, // TODO fill from Equipment service when it's done there
					FreePlacesQuantity = 0, // TODO TotalPlaces - (NumberOfEquipment + NumberOfEquipmentUnderRepair)
					Manager = new SearchFacilitiesUserResponse
					{
						Id = x.ManagerId,
						Name = usersDictionary.GetValueOrDefault(x.ManagerId)
					},
					DeputyManager = new SearchFacilitiesUserResponse
					{
						Id = x.DeputyManagerId,
						Name = usersDictionary.GetValueOrDefault(x.DeputyManagerId)
					},
					Technicians = x.TechnicianIds
						.Select(t => new SearchFacilitiesUserResponse
						{
							Id = t,
							Name = usersDictionary.GetValueOrDefault(t)
						})
						.ToArray()
				})
				.ToList()
		};
	}

	public async Task<Guid> Create(CreateFacilityRequest request, CancellationToken cancellationToken)
	{
		var userIds = new Guid[] { request.ManagerId, request.DeputyManagerId }
			.Union(request.TechnicianIds)
			.Distinct();

		var users = await userInternalApiClient.GetByIds(userIds, cancellationToken);

		var existingUserIds = users
			.Select(x => x.Id)
			.ToArray();

		var nonExistingUsers = userIds.Except(existingUserIds).ToList();
		if (nonExistingUsers.Any())
		{
			throw new ValidationException($"Users with ids \"{string.Join(',', nonExistingUsers)}\" don't exist.");
		}

		var requestDto = mapper.Map<CreateFacilityRequestDto>(request);

		return await facilityApiClient.Create(requestDto, cancellationToken);
	}

	public async Task Update(UpdateFacilityRequest request, CancellationToken cancellationToken)
	{
		var userIds = new Guid[] { request.ManagerId, request.DeputyManagerId }
			.Union(request.TechnicianIds)
			.Distinct();

		var users = await userInternalApiClient.GetByIds(userIds, cancellationToken);

		var existingUserIds = users
			.Select(x => x.Id)
			.ToArray();

		var nonExistingUsers = userIds.Except(existingUserIds).ToList();

		if (nonExistingUsers.Any())
		{
			throw new ValidationException($"Users with ids \"{string.Join(',', nonExistingUsers)}\" don't exist.");
		}

		var requestDto = mapper.Map<UpdateFacilityRequestDto>(request);

		await facilityApiClient.Update(requestDto, cancellationToken);
	}

	public async Task Archive(Guid id, CancellationToken cancellationToken)
	{
		await facilityApiClient.Archive(id, cancellationToken);
	}

	public async Task Unarchive(Guid id, CancellationToken cancellationToken)
	{
		await facilityApiClient.Unarchive(id, cancellationToken);
	}

	public async Task Delete(Guid id, CancellationToken cancellationToken)
	{
		await facilityApiClient.Delete(id, cancellationToken);
	}
}
