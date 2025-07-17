using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.Users.RemovePassword;

public record RemoveUserPasswordRequest(Guid Id) : IRequest;

public class RemoveUserPasswordHandler : IRequestHandler<RemoveUserPasswordRequest>
{
	private readonly IdentityUserManager _userManager;

	public RemoveUserPasswordHandler(IdentityUserManager userManager)
	{
		_userManager = userManager;
	}

	public async Task<Unit> Handle(RemoveUserPasswordRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.Id.ToString());
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		if (user.IsAdmin)
		{
			throw new ValidationException(
				"Нельзя сбрасывать пароль у пользователей, являющихся системными администраторами.");
		}

		await _userManager.RemovePasswordAsync(user);

		return Unit.Value;
	}
}
