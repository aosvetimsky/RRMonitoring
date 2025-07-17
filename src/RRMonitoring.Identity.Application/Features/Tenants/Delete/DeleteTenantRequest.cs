using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Tenants.Delete;

public class DeleteTenantRequest : IRequest
{
	public DeleteTenantRequest(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; set; }
}

public class DeleteTenantHandler : IRequestHandler<DeleteTenantRequest>
{
	private readonly ITenantRepository _tenantRepository;

	public DeleteTenantHandler(ITenantRepository tenantRepository)
	{
		_tenantRepository = tenantRepository;
	}

	public async Task<Unit> Handle(DeleteTenantRequest request, CancellationToken cancellationToken)
	{
		var tenant = await _tenantRepository.GetById(request.Id, cancellationToken: cancellationToken);

		if (tenant == null)
		{
			throw new ValidationException($"Tenant с ID: {request.Id} не найден.");
		}

		await _tenantRepository.Delete(tenant, cancellationToken);

		return Unit.Value;
	}
}
