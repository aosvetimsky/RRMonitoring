using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.RoleManager;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.Roles.Delete;

public class DeleteRoleRequest : IRequest
{
	public DeleteRoleRequest(Guid id)
	{
		Id = id;
	}

	public Guid Id { get; set; }
}

public class DeleteRoleHandler : IRequestHandler<DeleteRoleRequest>
{
	private readonly IdentityRoleManager _roleManager;
	private readonly IdentityUserManager _userManager;
	private const string RoleHasActiveUsersErrorCode = "RoleHasActiveUsers";

	public DeleteRoleHandler(
		IdentityRoleManager roleManager,
		IdentityUserManager userManager)
	{
		_roleManager = roleManager;
		_userManager = userManager;
	}

	public async Task<Unit> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
	{
		var role = await _roleManager.FindByIdAsync(request.Id.ToString());
		if (role is null)
		{
			throw new ValidationException("Роль с таким идентификатором не существует.");
		}

		if (role.IsSystem)
		{
			throw new ValidationException("Нельзя удалить системную роль.");
		}

		var usersInRole = await _userManager.GetUsersInRoleAsync(role.NormalizedName);
		if (usersInRole.Any(x => !x.IsBlocked))
		{
			throw new ValidationException("Нельзя удалить роль, к которой привязаны активные пользователи",
				errorCode: RoleHasActiveUsersErrorCode);
		}

		var result = await _roleManager.DeleteAsync(role);

		if (!result.Succeeded)
		{
			throw new Exception(
				$"Ошибка при удалении роли: {string.Join(",", result.Errors.Select(x => x.Description))}.");
		}

		return Unit.Value;
	}
}
