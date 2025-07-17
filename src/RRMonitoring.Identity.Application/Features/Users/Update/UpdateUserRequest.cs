using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Features.Users.Update;

public record UpdateUserRequest : IRequest
{
	public Guid Id { get; init; }
	public string Login { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string MiddleName { get; init; }
	public string Email { get; init; }
	public bool? EmailConfirmed { get; init; }
	public string PhoneNumber { get; set; }
	public bool? PhoneNumberConfirmed { get; init; }
	public string ExternalId { get; init; }
	public byte? UserTypeId { get; init; }
	public byte? ExternalSourceId { get; init; }
	public bool IsAgreementAcceptanceRequired { get; init; }
	public ICollection<Guid> RoleIds { get; init; }
	public ICollection<Guid> TenantIds { get; init; }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly IUserRepository _userRepository;
	private readonly IMapper _mapper;
	private readonly ILogger<UpdateUserHandler> _logger;

	public UpdateUserHandler(
		IdentityUserManager userManager,
		IUserRepository userRepository,
		IMapper mapper,
		ILogger<UpdateUserHandler> logger)
	{
		_userManager = userManager;
		_userRepository = userRepository;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Unit> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
	{
		var includes = new[] { nameof(User.UserRoles), nameof(User.UserTenants) };
		var user = await _userRepository.GetById(request.Id, includes, cancellationToken: cancellationToken);
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		var existingUserIds = await _userManager.GetExistingUserIds(request.Email, request.PhoneNumber, request.Login);

		if (existingUserIds?.Any() == true && (!existingUserIds.Contains(user.Id) || existingUserIds.Count > 1))
		{
			throw new ValidationException(
				"Сотрудник с такой почтой, телефоном и/или логином уже существует. Проверьте указанные данные.",
				new[]
				{
					new ValidationFailure(nameof(request.Email)),
					new ValidationFailure(nameof(request.PhoneNumber)), new ValidationFailure(nameof(request.Login))
				});
		}

		_mapper.Map(request, user);

		var result = await _userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to update user: {Error}", string.Join(", ", errorDescriptions));

			throw new ValidationException("Ошибка при обновлении данных пользователя.");
		}

		return Unit.Value;
	}
}
