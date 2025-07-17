using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Validators;

public class EmailUserValidator : IUserValidator<User>
{
	private readonly IdentityErrorDescriber _describer;

	public EmailUserValidator(IdentityErrorDescriber errors = null)
	{
		_describer = errors ?? new IdentityErrorDescriber();
	}

	public async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
	{
		var errors = new List<IdentityError>();

		if (string.IsNullOrWhiteSpace(user.Email)
		    && string.IsNullOrWhiteSpace(user.UserName)
		    && string.IsNullOrWhiteSpace(user.PhoneNumber))
		{
			errors.Add(new IdentityError
			{
				Description = "One of the fields must be filled: Email, UserName, PhoneNumber"
			});
		}

		if (!string.IsNullOrWhiteSpace(user.Email))
		{
			errors = await ValidateEmail(manager, user, errors).ConfigureAwait(false);
		}

		return errors?.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
	}

	private async Task<List<IdentityError>> ValidateEmail(
		UserManager<User> manager, User user, List<IdentityError> errors)
	{
		var email = await manager.GetEmailAsync(user).ConfigureAwait(false);
		if (string.IsNullOrWhiteSpace(email))
		{
			errors ??= new List<IdentityError>();
			errors.Add(_describer.InvalidEmail(email));
			return errors;
		}

		if (!new EmailAddressAttribute().IsValid(email))
		{
			errors ??= new List<IdentityError>();
			errors.Add(_describer.InvalidEmail(email));
			return errors;
		}

		var owner = await manager.FindByEmailAsync(email).ConfigureAwait(false);
		if (owner != null)
		{
			var ownerUserId = await manager.GetUserIdAsync(owner).ConfigureAwait(false);
			var userId = await manager.GetUserIdAsync(user).ConfigureAwait(false);
			if (ownerUserId != userId)
			{
				errors ??= new List<IdentityError>();
				errors.Add(_describer.DuplicateEmail(email));
			}
		}

		return errors;
	}
}
