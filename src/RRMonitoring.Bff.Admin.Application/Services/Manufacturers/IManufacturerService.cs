using System;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Manufacturers.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Manufacturers;

public interface IManufacturerService
{
	Task<ManufacturerByIdResponse> GetById(Guid id, CancellationToken cancellationToken);

	Task<PagedList<SearchManufacturersResponse>> Search(SearchManufacturersRequest request, CancellationToken cancellationToken);

	Task<Guid> Create(CreateManufacturerRequest request, CancellationToken cancellationToken);

	Task<Guid> Update(UpdateManufacturerRequest request, CancellationToken cancellationToken);

	Task Delete(Guid id, CancellationToken cancellationToken);
}