using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Configuration;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Application.Features.ForgotPassword.GetLink;
using RRMonitoring.Identity.Application.Features.Users.Approve;
using RRMonitoring.Identity.Application.Features.Users.ChangeStatus;
using RRMonitoring.Identity.Application.Features.Users.ChangeStatusRange;
using RRMonitoring.Identity.Application.Features.Users.CreateInternal;
using RRMonitoring.Identity.Application.Features.Users.Delete;
using RRMonitoring.Identity.Application.Features.Users.GetByExternalId;
using RRMonitoring.Identity.Application.Features.Users.GetById;
using RRMonitoring.Identity.Application.Features.Users.GetByUserNames;
using RRMonitoring.Identity.Application.Features.Users.GetProfileById;
using RRMonitoring.Identity.Application.Features.Users.PartialUpdateInternal;
using RRMonitoring.Identity.Application.Features.Users.RemovePassword;
using RRMonitoring.Identity.Application.Features.Users.Search;
using RRMonitoring.Identity.Application.Features.Users.SetPasswordInternal;
using RRMonitoring.Identity.Application.Features.Users.Update;
using SystemTextJsonPatch;

namespace RRMonitoring.Identity.Api.Controllers.Internal;

[InternalRoute("user")]
[ApiController]
[Authorize(AuthenticationSchemes = ApiKeyAuthenticationOptions.DefaultScheme)]
public class UserInternalController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserInternalController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	public Task<UserByIdResponse> GetById([FromRoute] Guid id)
	{
		return _mediator.Send(new GetUserByIdRequest(id));
	}

	[HttpPost("get-by-ids")]
	public Task<List<UserByIdResponse>> GetByIds([FromBody] IEnumerable<Guid> ids)
	{
		return _mediator.Send(new GetUsersByIdsRequest(ids));
	}

	[HttpGet("{id:guid}/profile")]
	public Task<UserProfileByIdResponse> GetUserProfileById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		return _mediator.Send(new GetUserProfileByIdRequest(id), cancellationToken);
	}

	[HttpGet("by-external-id/{externalId}")]
	public Task<UserByExternalIdResponse> GetByExternalId([FromRoute] string externalId)
	{
		return _mediator.Send(new GetUserByExternalIdRequest(externalId));
	}

	[HttpGet("{id:guid}/reset-password-link")]
	public Task<string> GetResetPasswordLink([FromRoute] Guid id)
	{
		return _mediator.Send(new GetResetPasswordLinkRequest { UserId = id });
	}

	[HttpPost("search")]
	public Task<PagedList<SearchUsersResponseItem>> Search([FromBody] SearchUsersRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPost("get-by-user-names")]
	public Task<List<UserByUserNameResponse>> GetByUserNames([FromBody] IEnumerable<string> userNames)
	{
		return _mediator.Send(new GetUsersByUserNameRequest(userNames));
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<Guid> CreateUser([FromBody] CreateUserInternalRequest request)
	{
		request.PhoneNumber = PhoneNumberHelper.ModifyPhoneCountryCode(request.PhoneNumber);

		return _mediator.Send(request);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
	{
		request.PhoneNumber = PhoneNumberHelper.ModifyPhoneCountryCode(request.PhoneNumber);

		await _mediator.Send(request);

		return Ok();
	}

	[HttpPatch("{id:guid}")]
	public Task PartialUpdateUser([FromRoute] Guid id,
		[FromBody] JsonPatchDocument<PartialUpdateUserInternalRequest> request,
		CancellationToken cancellationToken)
	{
		return _mediator.Send(new PartialUpdateUserInternalRequestDto(id, request), cancellationToken);
	}

	[HttpPut("set-password")]
	public Task SetPassword([FromBody] SetUserPasswordInternalRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPut("{id:guid}/block")]
	public async Task Block([FromRoute] Guid id, [FromBody] ChangeUserBlockRequest request)
	{
		await _mediator.Send(new ChangeUserBlockingStatusRequest(id, true, request.BlockReason, request.RemovePassword));
	}

	[HttpPut("block-range")]
	public async Task BlockRange([FromBody] List<Guid> ids)
	{
		await _mediator.Send(new ChangeUsersBlockingStatusRequest(ids, true));
	}

	[HttpPut("{id:guid}/unblock")]
	public async Task Unblock([FromRoute] Guid id)
	{
		await _mediator.Send(new ChangeUserBlockingStatusRequest(id, false));
	}

	[HttpPut("unblock-range")]
	public async Task UnblockRange([FromBody] List<Guid> ids)
	{
		await _mediator.Send(new ChangeUsersBlockingStatusRequest(ids, false));
	}

	[HttpPut("{id:guid}/approve")]
	public async Task Approve([FromRoute] Guid id)
	{
		await _mediator.Send(new ApproveUserRequest(id));
	}

	[HttpDelete("{id:guid}")]
	public async Task Delete(Guid id)
	{
		await _mediator.Send(new DeleteUserRequest(id));
	}

	[HttpDelete("{id:guid}/password")]
	public async Task RemovePassword(Guid id)
	{
		await _mediator.Send(new RemoveUserPasswordRequest(id));
	}
}
