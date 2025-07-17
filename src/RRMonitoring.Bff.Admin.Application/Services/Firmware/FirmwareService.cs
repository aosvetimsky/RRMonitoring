using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Firmware.Models;
using RRMonitoring.Equipment.ApiClients;
using RRMonitoring.Equipment.PublicModels.Firmware;
using RRMonitoring.Identity.ApiClients.ApiClients;

namespace RRMonitoring.Bff.Admin.Application.Services.Firmware;

public class FirmwareService(
	IFirmwareApiClient firmwareApiClient,
	IUserInternalApiClient userInternalApiClient,
	IMapper mapper) : IFirmwareService
{
	public async Task<FirmwareByIdResponse> GetById(Guid id, CancellationToken cancellationToken)
	{
		var firmware = await firmwareApiClient.GetById(id, cancellationToken);

		return mapper.Map<FirmwareByIdResponse>(firmware);
	}

	public Task<HttpResponseMessage> GetFile(Guid id, CancellationToken cancellationToken)
	{
		return firmwareApiClient.GetFile(id, cancellationToken);
	}

	public async Task<PagedList<SearchFirmwareResponse>> Search(SearchFirmwareRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<SearchFirmwareRequestDto>(request);
		requestDto.SortExpressions = new List<SortExpression>
		{
			new SortExpression { PropertyName = "CreatedDate", Direction = SortDirection.Desc }
		};

		var firmware = await firmwareApiClient.Search(requestDto, cancellationToken);

		var firmwareResponse = mapper.Map<PagedList<SearchFirmwareResponse>>(firmware);

		var userIds = firmwareResponse.Items
			.Select(x => x.CreatedBy)
			.Distinct()
			.ToList();

		var users = await userInternalApiClient.GetByIds(userIds, cancellationToken);

		foreach (var firmwareItem in firmwareResponse.Items)
		{
			firmwareItem.CreatedByName = users.FirstOrDefault(x => x.Id == firmwareItem.CreatedBy)?.Login;
		}

		return firmwareResponse;
	}

	public async Task<Guid> Create(CreateFirmwareRequest request, CancellationToken cancellationToken)
	{
		await using var stream = request.File.OpenReadStream();

		return await firmwareApiClient.Create(
			request.Name,
			request.Version,
			request.Comment,
			request.EquipmentModelIds.Select(x => x.ToString()),
			request.File == null ? null : new Refit.StreamPart(stream, fileName: request.File.FileName, contentType: request.File.ContentType),
			cancellationToken);
	}

	public async Task Update(UpdateFirmwareRequest request, CancellationToken cancellationToken)
	{
		var requestDto = mapper.Map<UpdateFirmwareRequestDto>(request);

		await firmwareApiClient.Update(requestDto, cancellationToken);
	}

	public async Task Delete(Guid id, CancellationToken cancellationToken)
	{
		await firmwareApiClient.Delete(id, cancellationToken);
	}
}
