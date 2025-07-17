using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Application.Enums;
using RRMonitoring.Identity.Application.Services.Link;
using RRMonitoring.Identity.Application.Services.UserManager;

namespace RRMonitoring.Identity.Application.Features.ForgotPassword.GetLink;

public record GetResetPasswordLinkRequest : IRequest<string>
{
	public Guid UserId { get; set; }

	public ResetPasswordErrorCodes? ErrorCode { get; set; }
}

internal class GetResetPasswordLinkHandler : IRequestHandler<GetResetPasswordLinkRequest, string>
{
	private readonly IdentityUserManager _userManager;
	private readonly ILinkService _linkService;

	public GetResetPasswordLinkHandler(
		IdentityUserManager userManager,
		ILinkService linkService)
	{
		_userManager = userManager;
		_linkService = linkService;
	}

	public async Task<string> Handle(GetResetPasswordLinkRequest request, CancellationToken cancellationToken)
	{
		var user = await _userManager.FindByIdAsync(request.UserId.ToString());
		if (user is null)
		{
			throw new ValidationException($"Пользователь с ID: '{request.UserId}' не найден.");
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

		return _linkService.GenerateResetPasswordLink(request.UserId, token, request.ErrorCode);
	}
}
