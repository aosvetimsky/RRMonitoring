using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.Users.Delete;

public class DeleteUserRequest : IRequest
{
	public Guid Id { get; }

	public DeleteUserRequest(Guid id)
	{
		Id = id;
	}
}

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest>
{
	private readonly IdentityUserManager _userManager;

	public DeleteUserHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<Unit> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.Id.ToString());
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		await _userManager.DeleteAsync(user);

		return Unit.Value;
	}
}
