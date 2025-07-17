using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RRMonitoring.Identity.Application.Services.RoleManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Identity.Application.Features.Roles.Update;

public class UpdateRoleRequest : IRequest
{
	[Required]
	public Guid Id { get; set; }

	[Required]
	public string Name { get; set; }

	public Guid? TenantId { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}

public class UpdateRoleHandler : IRequestHandler<UpdateRoleRequest>
{
	private readonly IdentityRoleManager _roleManager;
	private readonly IRoleRepository _roleRepository;
	private readonly IPermissionRepository _permissionRepository;
	private readonly ITenantRepository _tenantRepository;

	public UpdateRoleHandler(
		IdentityRoleManager roleManager,
		IRoleRepository roleRepository,
		IPermissionRepository permissionRepository,
		ITenantRepository tenantRepository)
	{
		_roleManager = roleManager;
		_roleRepository = roleRepository;
		_permissionRepository = permissionRepository;
		_tenantRepository = tenantRepository;
	}

	public async Task<Unit> Handle(UpdateRoleRequest request, CancellationToken cancellationToken)
	{
		var role = await _roleRepository.GetById(request.Id, new string[]
		{
			nameof(Role.RolePermissions)
		}, cancellationToken: cancellationToken);

		if (role == null)
		{
			throw new ValidationException("Роли с таким идентификатором не существует.");
		}

		if (role.Name != request.Name || role.TenantId != request.TenantId)
		{
			if (await _roleManager.RoleExistsAsync(request.Name, request.TenantId))
			{
				throw new ValidationException("Роль с таким названием уже есть в системе. Укажите другое название");
			}

			role.Name = request.Name;
		}

		var permissions = await _permissionRepository.GetByIds(request.PermissionIds, cancellationToken: cancellationToken);
		var missingPermissions = request.PermissionIds.Except(permissions.Select(x => x.Id));

		if (missingPermissions.Any())
		{
			throw new ValidationException($"Разрешений с такими идентификаторами не существует: " +
				$"{string.Join(", ", missingPermissions)}.");
		}

		role.RolePermissions = permissions
			.Select(x => new RolePermission { PermissionId = x.Id, RoleId = role.Id })
			.ToList();

		var tenant = request.TenantId != null
			? await _tenantRepository.GetById(request.TenantId.Value, cancellationToken: cancellationToken)
			: null;

		if (request.TenantId != null && tenant == null)
		{
			throw new ValidationException($"Tenant с ID: {request.TenantId} не найден.");
		}

		role.TenantId = tenant?.Id;

		var result = await _roleManager.UpdateAsync(role);

		if (!result.Succeeded)
		{
			throw new Exception($"Ошибка при обновлении роли: {string.Join(",", result.Errors.Select(x => x.Description))}.");
		}

		return Unit.Value;
	}
}
