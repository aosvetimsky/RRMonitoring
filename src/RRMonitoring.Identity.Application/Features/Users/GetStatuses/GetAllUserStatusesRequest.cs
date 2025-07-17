using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Users.GetStatuses;

public record GetAllUserStatusesRequest : IRequest<List<UserStatusResponse>>;

public class GetAllUserStatusesHandler : IRequestHandler<GetAllUserStatusesRequest, List<UserStatusResponse>>
{
	private readonly IUserStatusRepository _userStatusRepository;
	private readonly IMapper _mapper;

	public GetAllUserStatusesHandler(IUserStatusRepository userStatusRepository, IMapper mapper)
	{
		_userStatusRepository = userStatusRepository;
		_mapper = mapper;
	}

	public async Task<List<UserStatusResponse>> Handle(
		GetAllUserStatusesRequest request,
		CancellationToken cancellationToken)
	{
		var statuses = await _userStatusRepository.GetAll(cancellationToken: cancellationToken);

		return _mapper.Map<List<UserStatusResponse>>(statuses);
	}
}
