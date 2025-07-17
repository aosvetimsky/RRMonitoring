using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Tenants.Update;

public class UpdateTenantRequest : IRequest
{
	public Guid Id { get; set; }

	public string Name { get; set; }
}

public class UpdateTenantHandler : IRequestHandler<UpdateTenantRequest>
{
	private readonly ITenantRepository _tenantRepository;
	private readonly IMapper _mapper;

	public UpdateTenantHandler(
		ITenantRepository tenantRepository,
		IMapper mapper)
	{
		_tenantRepository = tenantRepository;
		_mapper = mapper;
	}

	public async Task<Unit> Handle(UpdateTenantRequest request, CancellationToken cancellationToken)
	{
		var tenant = await _tenantRepository.GetById(request.Id, cancellationToken: cancellationToken);

		if (tenant == null)
		{
			throw new ValidationException($"Tenant с ID: {request.Id} не найден.");
		}

		_mapper.Map(request, tenant);

		await _tenantRepository.Update(tenant, cancellationToken);

		return Unit.Value;
	}
}
