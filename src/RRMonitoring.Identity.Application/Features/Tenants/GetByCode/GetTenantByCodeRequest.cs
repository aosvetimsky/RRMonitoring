using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Tenants.GetByCode;

public record GetTenantByCodeRequest : IRequest<TenantByCodeResponse>
{
	public string Code { get; set; }
}

public class GetTenantByCodeHandler : IRequestHandler<GetTenantByCodeRequest, TenantByCodeResponse>
{
	private readonly ITenantRepository _tenantRepository;
	private readonly IMapper _mapper;

	public GetTenantByCodeHandler(
		ITenantRepository tenantRepository,
		IMapper mapper)
	{
		_tenantRepository = tenantRepository;
		_mapper = mapper;
	}

	public async Task<TenantByCodeResponse> Handle(GetTenantByCodeRequest request, CancellationToken cancellationToken)
	{
		var tenant = await _tenantRepository.GetByCode(request.Code);
		if (tenant is null)
		{
			throw new ResourceNotFoundException($"Tenant with code: {request.Code} not found");
		}

		return _mapper.Map<TenantByCodeResponse>(tenant);
	}
}
