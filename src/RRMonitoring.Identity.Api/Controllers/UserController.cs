using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nomium.Core.Models;
using Nomium.Core.Security.Attributes;
using RRMonitoring.Identity.Api.Helpers;
using RRMonitoring.Identity.Api.Security;
using RRMonitoring.Identity.Application.Features.Users.Approve;
using RRMonitoring.Identity.Application.Features.Users.ChangeStatus;
using RRMonitoring.Identity.Application.Features.Users.Create;
using RRMonitoring.Identity.Application.Features.Users.Delete;
using RRMonitoring.Identity.Application.Features.Users.GetById;
using RRMonitoring.Identity.Application.Features.Users.Search;
using RRMonitoring.Identity.Application.Features.Users.Update;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("user")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UserController : ControllerBase
{
	private readonly IMediator _mediator;

	public UserController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}")]
	[Permission(Permissions.UserRead)]
	public Task<UserByIdResponse> GetById([FromRoute] Guid id)
	{
		return _mediator.Send(new GetUserByIdRequest(id));
	}

	[HttpPost("get-by-ids")]
	[Permission(Permissions.UserRead)]
	public Task<List<UserByIdResponse>> GetByIds([FromBody] IEnumerable<Guid> ids)
	{
		return _mediator.Send(new GetUsersByIdsRequest(ids));
	}

	[HttpPost("search")]
	[Permission(Permissions.UserRead)]
	public Task<PagedList<SearchUsersResponseItem>> Search([FromBody] SearchUsersRequest request)
	{
		return _mediator.Send(request);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Permission(Permissions.UserManage)]
	public Task<Guid> CreateUser([FromBody] CreateUserRequest request)
	{
		request.PhoneNumber = PhoneNumberHelper.ModifyPhoneCountryCode(request.PhoneNumber);

		return _mediator.Send(request);
	}

	[HttpPut]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Permission(Permissions.UserManage)]
	public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
	{
		request.PhoneNumber = PhoneNumberHelper.ModifyPhoneCountryCode(request.PhoneNumber);

		await _mediator.Send(request);

		return Ok();
	}

	[HttpPut("{id:guid}/block")]
	[Permission(Permissions.UserBlock)]
	public async Task Block([FromRoute] Guid id, [FromBody] ChangeUserBlockRequest request)
	{
		await _mediator.Send(new ChangeUserBlockingStatusRequest(id, true, request.BlockReason, request.RemovePassword));
	}

	[HttpPut("{id:guid}/unblock")]
	[Permission(Permissions.UserBlock)]
	public async Task Unblock([FromRoute] Guid id)
	{
		await _mediator.Send(new ChangeUserBlockingStatusRequest(id, false));
	}

	[HttpPut("{id:guid}/approve")]
	[Permission(Permissions.UserManage)]
	public async Task Approve([FromRoute] Guid id)
	{
		await _mediator.Send(new ApproveUserRequest(id));
	}

	[HttpDelete("{id:guid}")]
	[Permission(Permissions.UserManage)]
	public async Task Delete(Guid id)
	{
		await _mediator.Send(new DeleteUserRequest(id));
	}
}
