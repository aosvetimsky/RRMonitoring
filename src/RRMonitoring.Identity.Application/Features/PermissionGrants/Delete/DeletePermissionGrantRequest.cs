using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.PermissionGrants.Delete;

public class DeletePermissionGrantRequest : IRequest
{
	public Guid Id { get; set; }

	public DeletePermissionGrantRequest(Guid id)
	{
		Id = id;
	}
}

public class DeletePermissionGrantHandler : IRequestHandler<DeletePermissionGrantRequest>
{
	private readonly IPermissionGrantRepository _permissionGrantRepository;

	public DeletePermissionGrantHandler(IPermissionGrantRepository permissionGrantRepository)
	{
		_permissionGrantRepository = permissionGrantRepository;
	}

	public async Task<Unit> Handle(DeletePermissionGrantRequest request, CancellationToken cancellationToken)
	{
		var permissionGrant = await _permissionGrantRepository
			.GetById(request.Id, cancellationToken: cancellationToken);

		if (permissionGrant == null)
		{
			throw new ValidationException($"Записи по делегированию с Id:{request.Id} не найдено.");
		}

		await _permissionGrantRepository.Delete(permissionGrant, cancellationToken);

		return Unit.Value;
	}
}
