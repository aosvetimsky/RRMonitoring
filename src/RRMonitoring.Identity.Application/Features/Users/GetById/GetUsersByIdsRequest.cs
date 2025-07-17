using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Users.GetById;

public class GetUsersByIdsRequest : IRequest<List<UserByIdResponse>>
{
	public GetUsersByIdsRequest(IEnumerable<Guid> ids)
	{
		Ids = ids;
	}

	public IEnumerable<Guid> Ids { get; set; }
}

public class GetUsersByIdsHandler : IRequestHandler<GetUsersByIdsRequest, List<UserByIdResponse>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUsersByIdsHandler(
		IUserRepository userRepository,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<List<UserByIdResponse>> Handle(GetUsersByIdsRequest request, CancellationToken cancellationToken)
	{
		var includes = new[]
		{
			$"{nameof(User.UserRoles)}" +
			$".{nameof(UserRole.Role)}",
			$"{nameof(User.Type)}", $"{nameof(User.Status)}", $"{nameof(User.UserTenants)}" +
			                                                  $".{nameof(TenantUser.Tenant)}"
		};

		var users = await _userRepository.GetByIds(request.Ids, includes,
			asNoTracking: true, cancellationToken: cancellationToken);

		return _mapper.Map<List<UserByIdResponse>>(users);
	}
}
