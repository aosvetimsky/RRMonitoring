using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Data.Transactions;
using Nomium.Core.Exceptions;
using Nomium.Core.FileStorage.Providers.S3;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.Domain.Entities;

namespace RRMonitoring.Equipment.Application.Features.Firmwares.Delete;

public record DeleteFirmwareRequest(Guid Id) : IRequest;

public class DeleteFirmwareHandler(
	IFirmwareRepository firmwareRepository,
	IS3FileProvider s3FileProvider,
	FirmwareUrlResolver firmwareUrlResolver,
	ITransactionScopeManager transactionScopeManager)
	: IRequestHandler<DeleteFirmwareRequest>
{
	public async Task Handle(DeleteFirmwareRequest request, CancellationToken cancellationToken)
	{
		var firmware = await firmwareRepository.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);

		if (firmware is null)
		{
			throw new ValidationException($"{nameof(Firmware)} with id: {request.Id} is not found.");
		}

		var filePath = firmwareUrlResolver.GetFilePathById(firmware.Id, firmware.OriginFileName);

		await transactionScopeManager.Execute(async () =>
		{
			await firmwareRepository.Delete(firmware, cancellationToken);

			await s3FileProvider.DeleteFile(filePath, cancellationToken: cancellationToken);
		}, cancellationToken);
	}
}
