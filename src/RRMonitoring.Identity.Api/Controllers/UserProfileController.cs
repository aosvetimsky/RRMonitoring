using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.FileStorage.Attributes;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Application.Constants;
using RRMonitoring.Identity.Application.Features.UserProfile.ChangePassword;
using RRMonitoring.Identity.Application.Features.UserProfile.Delete;
using RRMonitoring.Identity.Application.Features.UserProfile.GetInfo;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateEmail.SendCode;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateInfo;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhone.SendCode;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdatePhoto;
using RRMonitoring.Identity.Application.Features.UserProfile.UpdateUsername;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("user-profile")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserProfileController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserProfileController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<CurrentUserInfoResponse> GetCurrentUserInfo()
	{
		return await _mediator.Send(new GetCurrentUserInfoRequest());
	}

	[HttpPost("change-password")]
	public async Task ChangePassword([FromBody] ChangePasswordRequest request)
	{
		await _mediator.Send(request);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task UpdateCurrentUserInfo([FromBody] UpdateCurrentUserInfoRequest updateUserInfoRequest)
	{
		updateUserInfoRequest.PhoneNumber = PhoneNumberHelper.ModifyPhoneCountryCode(updateUserInfoRequest.PhoneNumber);

		await _mediator.Send(updateUserInfoRequest);
	}

	#region Change user email flow

	[HttpPost("email/verification-code")]
	public Task<int> SendCodeToNewEmail(
		[FromBody] SendCodeToNewUserEmailRequest request,
		CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	[HttpPut("email")]
	public async Task UpdateEmail(
		[FromBody] UpdateCurrentUserEmailRequest request,
		CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}

	#endregion

	#region Change phone number flow

	[HttpPut("phone")]
	public Task UpdatePhone(
		[FromBody] UpdateCurrentUserPhoneRequest request,
		CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	[HttpPost("phone/verification-code")]
	public Task<int> SendCodeToNewPhone(
		[FromBody] SendCodeToNewPhoneRequest request,
		CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	#endregion

	[HttpPut("username")]
	public Task UpdateUsername(
		[FromBody] UpdateCurrentUserUsernameRequest request,
		CancellationToken cancellationToken)
	{
		return _mediator.Send(request, cancellationToken);
	}

	[HttpPut("photo")]
	public async Task UploadPhoto(
		[FromForm]
		[AllowedFileExtensions(KnownFileExtensions.Jpeg, KnownFileExtensions.Jpg, KnownFileExtensions.Png)]
		[MaxFileSize(5)]
		IFormFile profilePhoto,
		CancellationToken cancellationToken)
	{
		await _mediator.Send(new UpdateCurrentUserPhotoRequest(profilePhoto), cancellationToken);
	}

	[HttpPut("deactivate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task DeactivateCurrentUser(
		[FromBody] DeleteCurrentUserRequest request,
		CancellationToken cancellationToken)
	{
		await _mediator.Send(request, cancellationToken);
	}
}
