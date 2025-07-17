using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Domain.Contracts.Repositories;

namespace RRMonitoring.Identity.Application.Features.UserTypes.GetAll;

public class GetUserTypesRequest : IRequest<IList<UserTypeResponse>>
{
}

public class GetUserTypesHandler : IRequestHandler<GetUserTypesRequest, IList<UserTypeResponse>>
{
	private readonly IUserTypeRepository _userTypeRepository;
	private readonly IMapper _mapper;

	public GetUserTypesHandler(IUserTypeRepository userTypeRepository, IMapper mapper)
	{
		_userTypeRepository = userTypeRepository;
		_mapper = mapper;
	}

	public async Task<IList<UserTypeResponse>> Handle(GetUserTypesRequest request, CancellationToken cancellationToken)
	{
		var userTypes = await _userTypeRepository.GetAll(cancellationToken: cancellationToken);

		return _mapper.Map<IList<UserTypeResponse>>(userTypes);
	}
}
