using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.RoleManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Roles.Create;

public class CreateRoleRequest : IRequest<Guid>
{
	public string Name { get; set; }

	public Guid? TenantId { get; set; }

	public IList<Guid> PermissionIds { get; set; }
}

public class CreateRoleHandler : IRequestHandler<CreateRoleRequest, Guid>
{
	private readonly IdentityRoleManager _roleManager;
	private readonly IPermissionRepository _permissionRepository;
	private readonly ITenantRepository _tenantRepository;

	public CreateRoleHandler(
		IdentityRoleManager roleManager,
		IPermissionRepository permissionRepository,
		ITenantRepository tenantRepository)
	{
		_roleManager = roleManager;
		_permissionRepository = permissionRepository;
		_tenantRepository = tenantRepository;
	}

	public async Task<Guid> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
	{
		if (await _roleManager.RoleExistsAsync(request.Name, request.TenantId))
		{
			throw new ValidationException("Роль с таким названием уже есть в системе. Укажите другое название");
		}

		var permissions = await _permissionRepository
			.GetByIds(request.PermissionIds, asNoTracking: true, cancellationToken: cancellationToken);

		var missingPermissionIds = request.PermissionIds
			.Except(permissions.Select(x => x.Id))
			.ToList();

		if (missingPermissionIds.Any())
		{
			throw new ValidationException($"Разрешений с такими идентификаторами не существует: " +
			                              $"{string.Join(", ", missingPermissionIds)}.");
		}

		var tenant = request.TenantId != null
			? await _tenantRepository.GetById(request.TenantId.Value, asNoTracking: true,
				cancellationToken: cancellationToken)
			: null;

		if (request.TenantId != null && tenant == null)
		{
			throw new ValidationException($"Tenant с ID: {request.TenantId} не существует");
		}

		var role = new Role
		{
			Name = request.Name,
			TenantId = tenant?.Id,
			RolePermissions = permissions
				.Select(x => new RolePermission { PermissionId = x.Id })
				.ToList()
		};

		var result = await _roleManager.CreateAsync(role);

		if (!result.Succeeded)
		{
			throw new Exception(
				$"Ошибка при создании роли: {string.Join(",", result.Errors.Select(x => x.Description))}");
		}

		return role.Id;
	}
}
