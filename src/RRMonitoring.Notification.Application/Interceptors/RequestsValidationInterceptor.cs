using System.Linq;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using NomiumValidationException = Nomium.Core.Exceptions.ValidationException;
using NomiumValidationFailure = Nomium.Core.Exceptions.ValidationFailure;

namespace RRMonitoring.Notification.Application.Interceptors;

public class RequestsValidationInterceptor : IValidatorInterceptor
{
	public ValidationResult AfterAspNetValidation(
		ActionContext actionContext, IValidationContext validationContext, ValidationResult result)
	{
		if (result.Errors.Any())
		{
			var validationFailures = result.Errors
				.Select(x => new NomiumValidationFailure(x.PropertyName, x.ErrorMessage));

			throw new NomiumValidationException("Validation of the input model is failed", validationFailures);
		}

		return result;
	}

	public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
	{
		return commonContext;
	}
}
