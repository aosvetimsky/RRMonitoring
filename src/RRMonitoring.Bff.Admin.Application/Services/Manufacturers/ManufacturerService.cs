using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;
using RRMonitoring.Equipment.ApiClients;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers;

public class ManufacturerService(IManufacturerApiClient manufacturerApiClient, IMapper mapper) : IManufacturerService
{
	public async Task<ManufacturerByIdResponse> GetById(Guid id, CancellationToken cancellationToken)
	{
		var pool = await manufacturerApiClient.GetById(id, cancellationToken);

		return mapper.Map<ManufacturerByIdResponse>(pool);
	}

	public async Task<PagedList<SearchManufacturersResponse>> Search(SearchManufacturersRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchManufacturersRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "Name", Direction = SortDirection.Asc }
		};

		var manufacturers = await manufacturerApiClient.Search(requestDto, cancellationToken);

		return mapper.Map<PagedList<SearchManufacturersResponse>>(manufacturers);
	}

	public async Task<Guid> Create(CreateManufacturerRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<CreateManufacturerRequestDto>(request);

		return await manufacturerApiClient.Create(requestDto, cancellationToken);
	}

	public async Task<Guid> Update(UpdateManufacturerRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<UpdateManufacturerRequestDto>(request);

		return await manufacturerApiClient.Update(requestDto, cancellationToken);
	}

	public async Task Delete(Guid id, CancellationToken cancellationToken)
	{
		await manufacturerApiClient.Delete(id, cancellationToken);
	}
}