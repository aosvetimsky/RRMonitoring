using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Tenants.GetById;

public class GetTenantByIdRequest : IRequest<TenantResponse>
{
	public GetTenantByIdRequest(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; set; }
}

public class GetTenantByIdHandler : IRequestHandler<GetTenantByIdRequest, TenantResponse>
{
	private readonly ITenantRepository _tenantRepository;
	private readonly IMapper _mapper;

	public GetTenantByIdHandler(
		ITenantRepository tenantRepository,
		IMapper mapper)
	{
		_tenantRepository = tenantRepository;
		_mapper = mapper;
	}

	public async Task<TenantResponse> Handle(GetTenantByIdRequest request, CancellationToken cancellationToken)
	{
		var tenant = await _tenantRepository.GetById(request.Id, cancellationToken: cancellationToken);

		if (tenant == null)
		{
			throw new ResourceNotFoundException($"Tenant с ID: {request.Id} не найден.");
		}

		return _mapper.Map<TenantResponse>(tenant);
	}
}
