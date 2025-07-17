using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Nomium.Core.Models;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Models;

namespace RRMonitoring.Identity.Application.Features.Users.Search;

public class SearchUsersRequest : PagedRequest, IRequest<PagedList<SearchUsersResponseItem>>
{
	public string Keyword { get; set; }
	public List<Guid> RoleIds { get; set; }
	public List<int> StatusIds { get; set; }
	public bool CanSeeSensitiveData { get; set; }
}

public class SearchUsersHandler : IRequestHandler<SearchUsersRequest, PagedList<SearchUsersResponseItem>>
{
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;

	public SearchUsersHandler(
		IUserRepository userRepository,
		IMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<PagedList<SearchUsersResponseItem>> Handle(
		SearchUsersRequest request,
		CancellationToken cancellationToken)
	{
		var searchCriteria = _mapper.Map<SearchUsersCriteria>(request);

		var users = await _userRepository.SearchUsers(searchCriteria, cancellationToken);
		ApplySensitiveDataPermissions(users, request.CanSeeSensitiveData);

		return _mapper.Map<PagedList<SearchUsersResponseItem>>(users);
	}

	public static void ApplySensitiveDataPermissions(PagedList<User> users, bool canSeeSensitiveData)
	{
		if (!canSeeSensitiveData)
		{
			foreach (var user in users.Items)
			{
				user.PhoneNumber = null;
				user.Email = null;
			}
		}
	}
}
