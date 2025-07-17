using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Tenants.Create;

public class CreateTenantRequest : IRequest<Guid>
{
	public string Name { get; set; }
}

public class CreateTenantHandler : IRequestHandler<CreateTenantRequest, Guid>
{
	private readonly ITenantRepository _tenantRepository;
	private readonly IMapper _mapper;

	public CreateTenantHandler(
		ITenantRepository tenantRepository,
		IMapper mapper)
	{
		_tenantRepository = tenantRepository;
		_mapper = mapper;
	}

	public async Task<Guid> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
	{
		var tenant = _mapper.Map<Tenant>(request);

		await _tenantRepository.Add(tenant, cancellationToken);

		return tenant.Id;
	}
}
