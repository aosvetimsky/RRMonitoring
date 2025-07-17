using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using SystemTextJsonPatch;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Identity.Application.Features.Users.PartialUpdateInternal;

public record PartialUpdateUserInternalRequestDto(
	Guid Id,
	JsonPatchDocument<PartialUpdateUserInternalRequest> PatchDocument) : IRequest;

public record PartialUpdateUserInternalRequest
{
	public string UserName { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string MiddleName { get; init; }
	public string Email { get; init; }
	public bool EmailConfirmed { get; init; }
	public string PhoneNumber { get; init; }
	public bool PhoneNumberConfirmed { get; init; }
	public string ExternalId { get; init; }
	public byte? TypeId { get; init; }
	public byte StatusId { get; init; }
	public byte? ExternalSourceId { get; init; }
	public bool IsAgreementAcceptanceRequired { get; init; }
	public IEnumerable<PartialUpdateUserRole> UserRoles { get; init; }
}

public record PartialUpdateUserRole(Guid RoleId);

public class PartialUpdateUserInternalHandler : IRequestHandler<PartialUpdateUserInternalRequestDto>
{
	private readonly IdentityUserManager _userManager;
	private readonly IUserRepository _userRepository;
	private readonly IUserTypeRepository _userTypeRepository;
	private readonly IValidator<User> _userValidator;
	private readonly IMapper _mapper;
	private readonly ILogger<PartialUpdateUserInternalHandler> _logger;

	public PartialUpdateUserInternalHandler(
		IdentityUserManager userManager,
		IUserRepository userRepository,
		IUserTypeRepository userTypeRepository,
		IValidator<User> userValidator,
		IMapper mapper,
		ILogger<PartialUpdateUserInternalHandler> logger)
	{
		_userManager = userManager;
		_userRepository = userRepository;
		_userTypeRepository = userTypeRepository;
		_userValidator = userValidator;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Unit> Handle(PartialUpdateUserInternalRequestDto request, CancellationToken cancellationToken)
	{
		var includePaths = new[] { nameof(User.UserRoles), nameof(User.UserTenants) };

		var user = await _userRepository.GetById(request.Id, includePaths, cancellationToken: cancellationToken);
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.Id}' не найден.");
		}

		var patch = _mapper.Map<JsonPatchDocument<User>>(request.PatchDocument);
		patch.ApplyTo(user);

		user.PhoneNumber = ModifyPhoneCountryCode(user.PhoneNumber);

		await Validate(user, cancellationToken);

		var result = await _userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to update user: {Error}", string.Join(", ", errorDescriptions));

			throw new ValidationException("Ошибка при обновлении данных пользователя.");
		}

		return Unit.Value;
	}

	private static string ModifyPhoneCountryCode(string phoneNumber)
	{
		return string.IsNullOrWhiteSpace(phoneNumber)
			? null
			: Regex.Replace(phoneNumber, "^8", "+7");
	}

	private async Task Validate(User user, CancellationToken cancellationToken)
	{
		var validationResult = await _userValidator.ValidateAsync(user, cancellationToken);
		if (!validationResult.IsValid)
		{
			var validationFailures = validationResult.Errors
				.Select(x => new ValidationFailure(x.PropertyName, x.ErrorMessage));

			throw new ValidationException("Error on user validation", validationFailures);
		}

		if (user.TypeId.HasValue &&
		    !await _userTypeRepository.IsExist(user.TypeId.Value, cancellationToken))
		{
			throw new ValidationException($"Тип пользователя с ID: '{user.TypeId}' не существует.");
		}

		var existingUserIds = await _userManager.GetExistingUserIds(user.Email, user.PhoneNumber, user.UserName);

		if (existingUserIds?.Any() is true && (!existingUserIds.Contains(user.Id) || existingUserIds.Count > 1))
		{
			throw new ValidationException(
				"Пользователь с такой почтой, телефоном и/или логином уже существует. Проверьте указанные данные.",
				new[]
				{
					new ValidationFailure(nameof(user.Email)), new ValidationFailure(nameof(user.PhoneNumber)),
					new ValidationFailure(nameof(user.UserName))
				});
		}
	}
}
