using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Nomium.Core.Application.Services.DateTimeProvider;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.Registration;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.BusEvents.Users;
using RRMonitoring.Identity.Domain.Contracts;
using RRMonitoring.Identity.Domain.Contracts.Repositories;
using RRMonitoring.Identity.Domain.Entities;
using RRMonitoring.Identity.Domain.Enums;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Identity.Application.Features.Users.Registration;

public class UserRegistrationRequest : IRequest
{
	public string Email { get; set; }
	public string UserName { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
}

public class UserRegistrationHandler : IRequestHandler<UserRegistrationRequest>
{
	private const string UserRoleCode = "pool_user";

	private readonly IdentityUserManager _userManager;
	private readonly IUserRegistrationService _userRegistrationService;
	private readonly ITransactionScopeManager _transactionService;
	private readonly ILinkService _linkService;
	private readonly IRoleRepository _roleRepository;
	private readonly IUserRepository _userRepository;
	private readonly IPublishEndpoint _publishEndpoint;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IMapper _mapper;
	private readonly ILogger<UserRegistrationHandler> _logger;

	public UserRegistrationHandler(
		IdentityUserManager userManager,
		IUserRegistrationService userRegistrationService,
		ITransactionScopeManager transactionService,
		ILinkService linkService,
		IRoleRepository roleRepository,
		IUserRepository userRepository,
		IPublishEndpoint publishEndpoint,
		IDateTimeProvider dateTimeProvider,
		IMapper mapper,
		ILogger<UserRegistrationHandler> logger)
	{
		_userManager = userManager;
		_userRepository = userRepository;
		_userRegistrationService = userRegistrationService;
		_transactionService = transactionService;
		_linkService = linkService;
		_roleRepository = roleRepository;
		_publishEndpoint = publishEndpoint;
		_dateTimeProvider = dateTimeProvider;
		_mapper = mapper;
		_logger = logger;
	}

	[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
	public async Task<Unit> Handle(UserRegistrationRequest request, CancellationToken cancellationToken)
	{
		await ValidateRequest(request, cancellationToken);

		var user = await CreateUser(request);

		await SaveUserAndSendLink(user, cancellationToken);
		await PublishEvents(user, cancellationToken);

		return Unit.Value;
	}

	private async Task ValidateRequest(UserRegistrationRequest request, CancellationToken cancellationToken)
	{
		var existingUserByEmail = await _userManager.GetByConfirmedEmail(request.Email, cancellationToken);
		if (existingUserByEmail is not null)
		{
			throw new ValidationException(
				"Email has been already used.",
				new[] { new ValidationFailure(nameof(request.Email)) });
		}

		var existingUserByUsername = await _userManager.FindByNameAsync(request.UserName);
		if (existingUserByUsername is not null)
		{
			throw new ValidationException(
				"Username has been already used.",
				new[] { new ValidationFailure(nameof(request.UserName)) });
		}
	}

	private async Task<User> CreateUser(UserRegistrationRequest request)
	{
		//TODO: fix roles logic

		// var roles = await _roleRepository.GetByCodes(new[] { UserRoleCode });
		// var poolUserRole = roles.First();

		var user = _mapper.Map<User>(request);
		user.StatusId = (byte)UserStatuses.Active;
		// user.UserRoles = new[] { new UserRole { RoleId = poolUserRole.Id } };

		var hasher = new PasswordHasher<User>();

		user.PasswordHash = hasher.HashPassword(user, request.Password);
		user.AgreementAcceptedDate = _dateTimeProvider.GetUtcNow();

		return user;
	}

	private async Task SaveUserAndSendLink(User user, CancellationToken cancellationToken)
	{
		await _transactionService.Execute(async () =>
		{
			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
			{
				var errorDescriptions = result.Errors.Select(x => x.Description);
				_logger.LogError("Errors when try to create user: {Error}", string.Join(", ", errorDescriptions));

				throw new ValidationException("Error on user creation.");
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

			var link = _linkService.GenerateConfirmEmailLink(user.Id, token);
			await _userRegistrationService.SendLink(user, link);
		}, cancellationToken);
	}

	private async Task PublishEvents(User user, CancellationToken cancellationToken)
	{
		var userCreatedEvent = new UserCreatedEvent { Id = user.Id };
		await _publishEndpoint.Publish(userCreatedEvent, cancellationToken);
	}
}
