using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Equipment.Models;
using RRMonitoring.Equipment.ApiClients;
using RRMonitoring.Equipment.PublicModels.Manufacturers;

namespace RRMonitoring.Bff.Admin.Application.Services.Equipment;

internal class EquipmentService(IManufacturerApiClient manufacturerApiClient, IMapper mapper) : IEquipmentService
{
	public async Task<PagedList<SearchEquipmentManufacturersResponse>> SearchManufacturers(SearchEquipmentManufacturersRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchManufacturersRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "Name", Direction = SortDirection.Asc }
		};

		var manufacturers = await manufacturerApiClient.Search(requestDto, cancellationToken);

		return mapper.Map<PagedList<SearchEquipmentManufacturersResponse>>(manufacturers);
	}
}
