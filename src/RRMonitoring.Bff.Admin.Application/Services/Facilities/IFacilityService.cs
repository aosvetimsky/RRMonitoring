using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Facilities.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Facilities;

public interface IFacilityService
{
	Task<FacilityByIdResponse> GetById(Guid id, CancellationToken cancellationToken);

	Task<PagedList<SearchFacilitiesResponse>> Search(SearchFacilitiesRequest request, CancellationToken cancellationToken);

	Task<Guid> Create(CreateFacilityRequest request, CancellationToken cancellationToken);

	Task Update(UpdateFacilityRequest request, CancellationToken cancellationToken);

	Task Archive(Guid id, CancellationToken cancellationToken);

	Task Unarchive(Guid id, CancellationToken cancellationToken);

	Task Delete(Guid id, CancellationToken cancellationToken);
}
