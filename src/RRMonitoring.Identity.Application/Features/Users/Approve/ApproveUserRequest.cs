using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RRMonitoring.Identity.Application.Features.ForgotPassword.SendLink;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Enums;

namespace RRMonitoring.Identity.Application.Features.Users.Approve;

public class ApproveUserRequest : IRequest
{
	public Guid Id { get; set; }

	public ApproveUserRequest(Guid id)
	{
		Id = id;
	}
}

public class ApproveUserHandler : IRequestHandler<ApproveUserRequest>
{
	private readonly IUserRepository _userRepository;
	private readonly IMediator _mediator;

	public ApproveUserHandler(
		IUserRepository userRepository,
		IMediator mediator)
	{
		_userRepository = userRepository;
		_mediator = mediator;
	}

	public async Task<Unit> Handle(ApproveUserRequest request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetById(request.Id, cancellationToken: cancellationToken);
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		if (user.StatusId != (byte)UserStatuses.OnApproval)
		{
			throw new ValidationException("Невозможно подтвердить пользователя, так как он находится в другом статусе.");
		}

		user.StatusId = (byte)UserStatuses.Active;

		await _userRepository.Update(user, cancellationToken);

		await _mediator.Send(new SendResetPasswordLinkRequest(user.Email), cancellationToken);

		return Unit.Value;
	}
}
