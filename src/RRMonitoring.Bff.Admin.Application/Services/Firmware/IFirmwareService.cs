using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware;

public interface IFirmwareService
{
	Task<FirmwareByIdResponse> GetById(Guid id, CancellationToken cancellationToken);

	Task<HttpResponseMessage> GetFile(Guid id, CancellationToken cancellationToken);

	Task<PagedList<SearchFirmwareResponse>> Search(SearchFirmwareRequest request, CancellationToken cancellationToken);

	Task<Guid> Create(CreateFirmwareRequest request, CancellationToken cancellationToken);

	Task Update(UpdateFirmwareRequest request, CancellationToken cancellationToken);

	Task Delete(Guid id, CancellationToken cancellationToken);
}
