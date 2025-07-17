using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RRMonitoring.Identity.Application.Configuration;
using RRMonitoring.Identity.Application.Services.UserManager;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Validators;

public class SameCharactersInLoginAndPasswordValidator : IPasswordValidator<User>
{
	public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
	{
		var identityUserManager = (IdentityUserManager)manager;
		var options = (RedRockIdentityOptions)identityUserManager.Options;
		if (!options.Password.MaxMatchingCredentialSymbolSequences.HasValue)
		{
			return Task.FromResult(IdentityResult.Success);
		}

		var chunkSize = options.Password.MaxMatchingCredentialSymbolSequences.Value + 1;
		var restrictedChunks = GetRestrictedChunks(user, chunkSize);
		var upperPassword = password.ToUpperInvariant();

		foreach (var chunk in restrictedChunks)
		{
			if (upperPassword.Contains(chunk))
			{
				var error = new IdentityError
				{
					Code = nameof(SameCharactersInLoginAndPasswordValidator),
					Description =
						"Пароль не соответствует политикам безопасности: содержит часть логина/имени пользователя"
				};

				return Task.FromResult(IdentityResult.Failed(error));
			}
		}

		return Task.FromResult(IdentityResult.Success);
	}

	private static List<string> GetRestrictedChunks(User user, int chunkSize)
	{
		var result = new List<string>();

		for (var i = 0; i < user.NormalizedUserName.Length - chunkSize - 1; i++)
		{
			var chunk = user.NormalizedUserName.Substring(i, chunkSize);
			result.Add(chunk);
		}

		var upperFullName = user.FullName.ToUpperInvariant();
		for (var i = 0; i < upperFullName.Length - chunkSize - 1; i++)
		{
			var chunk = upperFullName.Substring(i, chunkSize);
			result.Add(chunk);
		}

		return result;
	}
}
