using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Users.GetById;

public class GetUserByIdRequest : IRequest<UserByIdResponse>
{
	public Guid Id { get; }

	public GetUserByIdRequest(Guid id)
	{
		Id = id;
	}
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdRequest, UserByIdResponse>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public GetUserByIdHandler(
		IUserRepository userRepository,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<UserByIdResponse> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
	{
		var includes = new[]
		{
			$"{nameof(User.UserRoles)}" +
				$".{nameof(UserRole.Role)}",
			$"{nameof(User.Type)}",
			$"{nameof(User.Status)}",
			$"{nameof(User.UserTenants)}" +
				$".{nameof(TenantUser.Tenant)}",
			$"{nameof(User.ExternalSource)}"
		};

		var user = await _userRepository.GetById(request.Id, includes, asNoTracking: true, cancellationToken: cancellationToken);
		if (user is null)
		{
			throw new ResourceNotFoundException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		return _mapper.Map<UserByIdResponse>(user);
	}
}
