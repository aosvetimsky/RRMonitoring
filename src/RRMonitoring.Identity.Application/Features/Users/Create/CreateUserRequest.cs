using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Identity.Application.Features.Users.CreateInternal;

namespace RRMonitoring.Identity.Application.Features.Users.Create;

public record CreateUserRequest : IRequest<Guid>
{
	public string Login { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string MiddleName { get; init; }
	public string Email { get; init; }
	public bool EmailConfirmed { get; init; }
	public string PhoneNumber { get; set; }
	public bool PhoneNumberConfirmed { get; init; }
	public byte? UserTypeId { get; init; }
	public bool IsAgreementAcceptanceRequired { get; init; }
	public bool IsResetPasswordLinkSendingRequired { get; init; }
	public ICollection<Guid> RoleIds { get; init; }
	public ICollection<Guid> TenantIds { get; init; }
}

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Guid>
{
	private readonly IMapper _mapper;
	private readonly IMediator _mediator;

	public CreateUserHandler(IMapper mapper, IMediator mediator)
	{
		_mapper = mapper;
		_mediator = mediator;
	}

	public Task<Guid> Handle(CreateUserRequest request, CancellationToken cancellationToken)
	{
		var createUserInternalRequest = _mapper.Map<CreateUserInternalRequest>(request);

		return _mediator.Send(createUserInternalRequest, cancellationToken);
	}
}
