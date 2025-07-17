using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Users.GetByExternalId;

public record GetUserByExternalIdRequest(string ExternalId) : IRequest<UserByExternalIdResponse>;

public class GetUserByExternalIdHandler : IRequestHandler<GetUserByExternalIdRequest, UserByExternalIdResponse>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUserByExternalIdHandler(
		IUserRepository userRepository,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<UserByExternalIdResponse> Handle(
		GetUserByExternalIdRequest request,
		CancellationToken cancellationToken)
	{
		var includes = new[] { $"{nameof(User.Type)}", $"{nameof(User.Status)}" };

		var user = await _userRepository.GetByExternalId(request.ExternalId, includes,
			cancellationToken: cancellationToken);

		return _mapper.Map<UserByExternalIdResponse>(user);
	}
}
