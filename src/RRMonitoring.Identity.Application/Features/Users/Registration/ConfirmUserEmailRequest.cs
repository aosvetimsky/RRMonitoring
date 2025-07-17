using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using RRMonitoring.Identity.Application.Services.UserManager;
using ValidationException = Nomium.Core.Exceptions.ValidationException;

namespace RRMonitoring.Identity.Application.Features.Users.Registration;

public record ConfirmUserEmailRequest(string UserId, string Token) : IRequest;

public class ConfirmUserEmailHandler : IRequestHandler<ConfirmUserEmailRequest>
{
	private const string WrongLinkErrorMessage = "Ссылка имеет неверный формат";

	private readonly IdentityUserManager _userManager;
	private readonly ILogger<ConfirmUserEmailHandler> _logger;

	public ConfirmUserEmailHandler(
		IdentityUserManager userManager,
		ILogger<ConfirmUserEmailHandler> logger)
	{
		_userManager = userManager;
		_logger = logger;
	}

	public async Task<Unit> Handle(ConfirmUserEmailRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
		{
			_logger.LogWarning("Try to get user with ID: {UserId}. User doesn't exist", request.UserId);

			throw new ValidationException(WrongLinkErrorMessage);
		}

		var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
		var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
		if (!confirmResult.Succeeded)
		{
			var errorMessage = string.Join(',', confirmResult.Errors.Select(e => e.Description));

			throw new ValidationException("Error on token validation. Message: {ErrorMessage}", errorMessage);
		}

		return Unit.Value;
	}
}
