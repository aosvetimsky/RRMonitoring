using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.Users.GetByUserNames;

public record GetUsersByUserNameRequest(IEnumerable<string> UserNames) : IRequest<List<UserByUserNameResponse>>;

public class GetUsersByUserNameHandler : IRequestHandler<GetUsersByUserNameRequest, List<UserByUserNameResponse>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUsersByUserNameHandler(
		IUserRepository userRepository,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<List<UserByUserNameResponse>> Handle(
		GetUsersByUserNameRequest request,
		CancellationToken cancellationToken)
	{
		var users = await _userRepository.GetByUserNames(request.UserNames, cancellationToken);

		return _mapper.Map<List<UserByUserNameResponse>>(users);
	}
}
