using System;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Nomium.Core.Application.Services.DateTimeProvider;

namespace RRMonitoring.Identity.Application.Services.Agreement;

public class VerifiedLoginService : IVerifiedLoginService
{
	private const string VerifiedLoginCookie = "verifiedLogin";

	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly ISigningCredentialStore _signingCredentialStore;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly CryptoProviderFactory _cryptoProviderFactory;

	private readonly TimeSpan _loginCookieLifeSpan = TimeSpan.FromMinutes(5);

	public VerifiedLoginService(
		IHttpContextAccessor httpContextAccessor,
		ISigningCredentialStore signingCredentialStore,
		IDateTimeProvider dateTimeProvider,
		CryptoProviderFactory cryptoProviderFactory)
	{
		_httpContextAccessor = httpContextAccessor;
		_signingCredentialStore = signingCredentialStore;
		_dateTimeProvider = dateTimeProvider;
		_cryptoProviderFactory = cryptoProviderFactory;
	}

	public async Task RememberVerifiedLoginAsync(string userId)
	{
		var verifiedLogin = new VerifiedLogin
		{
			ExpirationDate = _dateTimeProvider.GetUtcNow().Add(_loginCookieLifeSpan), UserId = userId
		};
		var signatureBase64 = await CalculateSignatureBase64Async(verifiedLogin);

		var signedLogin = new SignedVerifiedLogin { Login = verifiedLogin, Signature = signatureBase64 };
		var signedLoginBase64 = ObjectToBase64(signedLogin);

		var options = new CookieOptions { HttpOnly = true };
		_httpContextAccessor.HttpContext.Response.Cookies.Append(VerifiedLoginCookie, signedLoginBase64, options);
	}

	public async Task<string> GetVerifiedUserIdAsync()
	{
		var signedLoginBase64 = _httpContextAccessor.HttpContext.Request.Cookies[VerifiedLoginCookie];

		if (!signedLoginBase64.IsNullOrEmpty())
		{
			var signedLogin = Base64ToObject<SignedVerifiedLogin>(signedLoginBase64);

			if (await IsLoginValidAsync(signedLogin))
			{
				return signedLogin.Login.UserId;
			}
		}

		return null;
	}

	public void ForgetVerifiedLogin()
	{
		_httpContextAccessor.HttpContext.Response.Cookies.Delete(VerifiedLoginCookie);
	}

	private async Task<string> CalculateSignatureBase64Async(VerifiedLogin login)
	{
		var verifiedLoginBytes = ObjectToBytes(login);
		var key = await _signingCredentialStore.GetSigningCredentialsAsync();
		var signatureProvider = _cryptoProviderFactory.CreateForSigning(key.Key, key.Algorithm);
		var signature = signatureProvider.Sign(verifiedLoginBytes);

		return Convert.ToBase64String(signature);
	}

	private async Task<bool> IsLoginValidAsync(SignedVerifiedLogin signedLogin)
	{
		var validSignature = await VerifyLoginSignatureAsync(signedLogin.Login, signedLogin.Signature);
		var expired = signedLogin.Login.ExpirationDate <= _dateTimeProvider.GetUtcNow();

		return validSignature && !expired;
	}

	private async Task<bool> VerifyLoginSignatureAsync(VerifiedLogin login, string signatureBase64)
	{
		var signatureBytes = Convert.FromBase64String(signatureBase64);
		var loginBytes = ObjectToBytes(login);

		var key = await _signingCredentialStore.GetSigningCredentialsAsync();
		var signatureProvider = _cryptoProviderFactory.CreateForVerifying(key.Key, key.Algorithm);
		return signatureProvider.Verify(loginBytes, signatureBytes);
	}

	private static byte[] ObjectToBytes<T>(T obj)
	{
		var json = JsonConvert.SerializeObject(obj);
		return Encoding.UTF8.GetBytes(json);
	}

	private static string ObjectToBase64<T>(T obj)
	{
		var bytes = ObjectToBytes(obj);
		return Convert.ToBase64String(bytes);
	}

	private static T Base64ToObject<T>(string base64)
	{
		var bytes = Convert.FromBase64String(base64);
		var json = Encoding.UTF8.GetString(bytes);
		return JsonConvert.DeserializeObject<T>(json);
	}
}
