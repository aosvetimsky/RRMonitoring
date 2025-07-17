using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Roles.GetByCode;

public record GetRolesByCodesRequest(IEnumerable<string> Codes) : IRequest<List<RolesByCodesResponse>>;

public class GetRoleByCodeHandler : IRequestHandler<GetRolesByCodesRequest, List<RolesByCodesResponse>>
{
	private readonly IRoleRepository _roleRepository;
	private readonly IMapper _mapper;

	public GetRoleByCodeHandler(IRoleRepository roleRepository, IMapper mapper)
	{
		_roleRepository = roleRepository;
		_mapper = mapper;
	}

	public async Task<List<RolesByCodesResponse>> Handle(
		GetRolesByCodesRequest request,
		CancellationToken cancellationToken)
	{
		var roles = await _roleRepository.GetByCodes(request.Codes,
			new[] { nameof(Role.RolePermissions), nameof(Role.Tenant) },
			cancellationToken);

		return _mapper.Map<List<RolesByCodesResponse>>(roles);
	}
}
