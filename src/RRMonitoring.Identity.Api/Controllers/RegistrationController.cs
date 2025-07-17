using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nomium.Core.Exceptions;
using RRMonitoring.Identity.Api.Attributes;
using RRMonitoring.Identity.Api.ViewModels;
using RRMonitoring.Identity.Application.Features.Users.Registration;

namespace RRMonitoring.Identity.Api.Controllers;

[Route("registration")]
[ApiController]
public class RegistrationController : Controller
{
	private readonly IMediator _mediator;
	private readonly ILogger<RegistrationController> _logger;
	private readonly string _loginRedirectUrl;

	public RegistrationController(
		IMediator mediator,
		IConfiguration configuration,
		ILogger<RegistrationController> logger)
	{
		_mediator = mediator;
		_logger = logger;

		_loginRedirectUrl = configuration.GetValue<string>("DefaultRedirectUrls:LoginPage");
	}

	[HttpGet]
	public IActionResult GetRegistrationForm([FromQuery(Name = "referrer")] string referrer)
	{
		var registerViewModel = new RegisterViewModel
		{
			LoginUrl = _loginRedirectUrl,
			ReferralCode = referrer
		};

		return View("~/Views/Registration/Registration.cshtml", registerViewModel);
	}

	[HttpPost]
	[SuppressModelStateInvalidFilter]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> Register([FromForm] RegisterViewModel registerViewModel)
	{
		if (!ModelState.IsValid)
		{
			registerViewModel.ErrorList = ModelState.Values
				.Where(x => x.ValidationState == ModelValidationState.Invalid)
				.SelectMany(x => x.Errors)
				.Select(x => x.ErrorMessage)
				.ToList();

			return View("~/Views/Registration/Registration.cshtml", registerViewModel);
		}

		try
		{
			if (!registerViewModel.IsTermsConfirmed)
			{
				throw new ValidationException("Подтвердите согласие с пользовательским соглашением");
			}

			var createUserRequest = new UserRegistrationRequest
			{
				Email = registerViewModel.Email,
				UserName = registerViewModel.UserName,
				Password = registerViewModel.Password,
				ConfirmPassword = registerViewModel.ConfirmPassword
			};

			await _mediator.Send(createUserRequest);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception when registering with email: {Email}", registerViewModel.Email);

			registerViewModel.ErrorList = ex is ValidationException vex ? vex.Errors.Select(x => x.ErrorMessage).ToList() : new List<string>();

			if (!string.IsNullOrEmpty(ex.Message))
			{
				registerViewModel.ErrorList.Add(ex.Message);
			}

			return View("~/Views/Registration/Registration.cshtml", registerViewModel);
		}

		var registerSuccessfulViewModel = new RegistrationSuccessfulViewModel
		{
			Email = registerViewModel.Email,
			LoginUrl = _loginRedirectUrl
		};

		return View("~/Views/Registration/RegistrationSuccessful.cshtml", registerSuccessfulViewModel);
	}

	[HttpGet("confirmation")]
	[SuppressMessage("Design", "CA1031:Do not catch general exception types")] // TODO: Catch specific exception
	public async Task<IActionResult> ConfirmRegistration([FromQuery] string userId, [FromQuery] string token)
	{
		try
		{
			await _mediator.Send(new ConfirmUserEmailRequest(userId, token));
		}
		catch (Exception)
		{
			return View("~/Views/Registration/RegistrationConfirmation.cshtml", new RegistrationConfirmationViewModel());
		}

		var emailConfirmationSuccessfulViewModel = new RegistrationConfirmationViewModel
		{
			IsTokenValid = true,
			LoginUrl = _loginRedirectUrl
		};

		return View("~/Views/Registration/RegistrationConfirmation.cshtml", emailConfirmationSuccessfulViewModel);
	}
}
