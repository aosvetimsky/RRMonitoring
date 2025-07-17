using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;
using RRMonitoring.Notification.ApiClients.Enums;
using RRMonitoring.Notification.ApiClients.Service.Notification.Http;

namespace RRMonitoring.Identity.Application.Features.Users.CreateInternal;

public sealed record CreateUserInternalRequest : IRequest<Guid>
{
	public string Login { get; init; }
	public string FirstName { get; init; }
	public string LastName { get; init; }
	public string MiddleName { get; init; }
	public string Email { get; init; }
	public bool EmailConfirmed { get; init; }
	public string PhoneNumber { get; set; }
	public bool PhoneNumberConfirmed { get; init; }
	public string ExternalId { get; init; }
	public bool IsBlocked { get; init; }
	public byte? StatusId { get; init; }
	public byte? UserTypeId { get; init; }
	public byte? ExternalSourceId { get; init; }
	public bool IsAgreementAcceptanceRequired { get; init; }
	public DateTime? AgreementAcceptedDate { get; init; }
	public bool IsResetPasswordLinkSendingRequired { get; init; }
	public ICollection<Guid> RoleIds { get; init; }
	public ICollection<Guid> TenantIds { get; init; }
}

public class CreateUserInternalHandler : IRequestHandler<CreateUserInternalRequest, Guid>
{
	private readonly IdentityUserManager _userManager;
	private readonly ILinkService _linkService;
	private readonly IHttpNotificationManager _httpNotificationManager;
	private readonly IRoleRepository _roleRepository;
	private readonly IUserTypeRepository _userTypeRepository;
	private readonly IExternalSourceRepository _externalSourceRepository;
	private readonly IMapper _mapper;
	private readonly ILogger<CreateUserInternalHandler> _logger;

	public CreateUserInternalHandler(
		IdentityUserManager userManager,
		ILinkService linkService,
		IHttpNotificationManager httpNotificationManager,
		IRoleRepository roleRepository,
		IUserTypeRepository userTypeRepository,
		IExternalSourceRepository externalSourceRepository,
		IMapper mapper,
		ILogger<CreateUserInternalHandler> logger)
	{
		_userManager = userManager;
		_linkService = linkService;
		_httpNotificationManager = httpNotificationManager;
		_roleRepository = roleRepository;
		_userTypeRepository = userTypeRepository;
		_externalSourceRepository = externalSourceRepository;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Guid> Handle(CreateUserInternalRequest request, CancellationToken cancellationToken)
	{
		var existingUserIds = await _userManager.GetExistingUserIds(request.Email, request.PhoneNumber, request.Login);
		if (existingUserIds?.Any() == true)
		{
			throw new ValidationException(
				"Сотрудник с такой почтой, телефоном и/или логином уже существует. Проверьте указанные данные.",
				new[]
				{
					new ValidationFailure(nameof(request.Email)),
					new ValidationFailure(nameof(request.PhoneNumber)), new ValidationFailure(nameof(request.Login))
				});
		}

		var user = _mapper.Map<User>(request);
		user.StatusId = request.StatusId ?? (byte)UserStatuses.Active;

		if (request.RoleIds?.Any() == true)
		{
			var roles = await _roleRepository.GetByIds(request.RoleIds, cancellationToken: cancellationToken);
			if (roles.Count != request.RoleIds.Count)
			{
				var notExistingRoles = request.RoleIds.Except(roles.Select(x => x.Id));

				throw new ValidationException($"Роли с ID: {string.Join(',', notExistingRoles)} не существуют");
			}

			user.UserRoles = request.RoleIds
				.Select(x => new UserRole { RoleId = x })
				.ToArray();
		}

		if (request.TenantIds?.Any() == true)
		{
			user.UserTenants = request.TenantIds
				.Select(s => new TenantUser { TenantId = s }).ToArray();
		}

		if (request.UserTypeId.HasValue &&
		    !await _userTypeRepository.IsExist(request.UserTypeId.Value, cancellationToken))
		{
			throw new ValidationException($"Тип пользователя с ID: {request.UserTypeId} не существует");
		}

		if (request.ExternalSourceId.HasValue &&
		    !await _externalSourceRepository.IsExist(request.ExternalSourceId.Value, cancellationToken))
		{
			throw new ValidationException($"Внешний источник с ID: {request.ExternalSourceId} не существует");
		}

		var result = await _userManager.CreateAsync(user);

		if (!result.Succeeded)
		{
			var errorDescriptions = result.Errors.Select(x => x.Description);
			_logger.LogError("Errors when try to create user: {Error}", string.Join(", ", errorDescriptions));

			throw new ValidationException("Ошибка при создании пользователя.");
		}

		if (request.IsResetPasswordLinkSendingRequired)
		{
			await SendCreateUserNotification(user);
		}

		return user.Id;
	}

	private async Task SendCreateUserNotification(User user)
	{
		if (string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.PhoneNumber))
		{
			return;
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		var link = _linkService.GenerateResetPasswordLink(user.Id, token, null);

		var channel = !string.IsNullOrEmpty(user.Email)
			? Channels.Email
			: Channels.Sms;

		var notification = new CreateUserNotification(channel)
		{
			UserFullName = user.FullName,
			Url = link,
			RecipientId = user.Id,
			Recipient = channel == Channels.Email
				? user.Email
				: user.PhoneNumber,
		};

		await _httpNotificationManager.SendMultiple(new[] { notification });
	}
}
