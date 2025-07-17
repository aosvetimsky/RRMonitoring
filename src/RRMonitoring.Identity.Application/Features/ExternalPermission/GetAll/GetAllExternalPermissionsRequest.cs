using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.ExternalPermission.GetAll;

public record GetAllExternalPermissionsRequest : IRequest<List<ExternalPermissionResponse>>;

public class GetAllExternalPermissionsHandler
	: IRequestHandler<GetAllExternalPermissionsRequest, List<ExternalPermissionResponse>>
{
	private readonly IExternalPermissionRepository _externalPermissionRepository;
	private readonly IMapper _mapper;

	public GetAllExternalPermissionsHandler(IExternalPermissionRepository externalPermissionRepository, IMapper mapper)
	{
		_externalPermissionRepository = externalPermissionRepository;
		_mapper = mapper;
	}

	public async Task<List<ExternalPermissionResponse>> Handle(
		GetAllExternalPermissionsRequest request,
		CancellationToken cancellationToken)
	{
		var permissions = await _externalPermissionRepository.GetAll(cancellationToken: cancellationToken);

		return _mapper.Map<List<ExternalPermissionResponse>>(permissions);
	}
}
