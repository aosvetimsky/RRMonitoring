using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using Refit;
using RRMonitoring.Equipment.PublicModels.Firmware;
using static System.Net.Mime.MediaTypeNames;

namespace RRMonitoring.Equipment.ApiClients;

public interface IFirmwareApiClient
{
	[Get("/v1/firmware/{id}")]
	Task<FirmwareByIdResponseDto> GetById(
		Guid id,
		CancellationToken cancellationToken);

	[Get("/v1/firmware/{id}/file")]
	Task<HttpResponseMessage> GetFile(
		Guid id,
		CancellationToken cancellationToken);

	[Post("/v1/firmware/search")]
	Task<PagedList<SearchFirmwareResponseDto>> Search(
		[Body] SearchFirmwareRequestDto requestDto,
		CancellationToken cancellationToken);

	[Post("/v1/firmware")]
	[Multipart]
	// Multipart doesn't allow to put this params into a complex model
	Task<Guid> Create(
		string name,
		string version,
		string comment,
		IEnumerable<string> equipmentModelIds,
		StreamPart file,
		CancellationToken cancellationToken);

	[Put("/v1/firmware")]
	Task<Guid> Update(
		[Body] UpdateFirmwareRequestDto requestDto,
		CancellationToken cancellationToken);

	[Delete("/v1/firmware/{id}")]
	Task Delete(
		Guid id,
		CancellationToken cancellationToken);
}
