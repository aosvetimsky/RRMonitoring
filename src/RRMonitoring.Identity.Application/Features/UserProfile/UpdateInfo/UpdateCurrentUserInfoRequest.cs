using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using Nomium.Core.Security.Services.CurrentUser;
using Nomium.Core.Security.Services.CurrentUser.Models;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.UserProfile.UpdateInfo;

public class UpdateCurrentUserInfoRequest : IRequest
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string TelegramLogin { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
}

public class UpdateCurrentUserInfoHandler : IRequestHandler<UpdateCurrentUserInfoRequest>
{
	private readonly IdentityUserManager _userManager;
	private readonly ICurrentUserService<CurrentUserBase> _currentUserService;
	private readonly IMapper _mapper;
	private readonly ILogger<UpdateCurrentUserInfoHandler> _logger;

	public UpdateCurrentUserInfoHandler(
		IdentityUserManager userManager,
		ICurrentUserService<CurrentUserBase> currentUserService,
		IMapper mapper,
		ILogger<UpdateCurrentUserInfoHandler> logger)
	{
		_userManager = userManager;
		_currentUserService = currentUserService;
		_mapper = mapper;
		_logger = logger;
	}

	public async Task<Unit> Handle(UpdateCurrentUserInfoRequest request, CancellationToken cancellationToken)
	{
		var currentUserId = _currentUserService.CurrentUserId;

		var currentUserInfo = await _userManager.FindByIdAsync(currentUserId.ToString());
		if (currentUserInfo == null)
		{
			throw new ValidationException($"Пользователь с ID: '{currentUserId}' не найден.");
		}

		var existingUserIds = await _userManager.GetExistingUserIds(request.Email, request.PhoneNumber, null);

		if (existingUserIds?.Any() == true && (!existingUserIds.Contains(currentUserId) || existingUserIds.Count > 1))
		{
			throw new ValidationException(
				"Сотрудник с такой почтой и/или телефоном уже существует. Проверьте указанные данные.",
				new[]
				{
					new ValidationFailure(nameof(request.Email)),
					new ValidationFailure(nameof(request.PhoneNumber)),
				});
		}

		_mapper.Map(request, currentUserInfo);

		var result = await _userManager.UpdateAsync(currentUserInfo);

		if (result.Succeeded)
		{
			return Unit.Value;
		}

		var errorDescriptions = result.Errors.Select(x => x.Description);
		_logger.LogError("Errors when try to update user: {Error}", string.Join(", ", errorDescriptions));

		throw new ValidationException("Ошибка при обновлении данных пользователя.");
	}
}
