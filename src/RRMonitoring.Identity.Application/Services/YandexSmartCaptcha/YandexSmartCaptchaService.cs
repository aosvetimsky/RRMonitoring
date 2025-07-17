using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RRMonitoring.Identity.Application.Configuration;

namespace RRMonitoring.Identity.Application.Services.YandexSmartCaptcha;

public class YandexSmartCaptchaService
{
	private readonly YandexSmartCaptchaConfiguration _configuration;
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly ILogger<YandexSmartCaptchaService> _logger;

	public YandexSmartCaptchaService(
		IOptions<YandexSmartCaptchaConfiguration> yandexSmartCaptchaOptions,
		IHttpClientFactory httpClientFactory,
		ILogger<YandexSmartCaptchaService> logger)
	{
		_configuration = yandexSmartCaptchaOptions.Value;
		_httpClientFactory = httpClientFactory;
		_logger = logger;
	}

	public bool EnabledOnLogin => _configuration.Enabled && _configuration.EnabledOnLogin;
	public string ClientSecretIfEnabledOnLogin => EnabledOnLogin ? _configuration.ClientSecret : null;

	public bool EnabledOnForgotPassword => _configuration.Enabled && _configuration.EnabledOnForgotPassword;
	public string ClientSecretIfEnabledOnForgotPassword => EnabledOnForgotPassword ? _configuration.ClientSecret : null;

	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
	public async Task<YandexSmartCaptchaValidationResultCode> ValidateSmartToken(
		string smartToken, CancellationToken cancellationToken)
	{
		if (string.IsNullOrEmpty(smartToken))
		{
			return YandexSmartCaptchaValidationResultCode.NoSmartToken;
		}

		using var client = _httpClientFactory.CreateClient();

		var url = QueryHelpers.AddQueryString("https://smartcaptcha.yandexcloud.net/validate",
			new Dictionary<string, string> { { "secret", _configuration.ServerSecret }, { "token", smartToken }, });

		try
		{
			var response = await client.GetAsync(url, cancellationToken);

			var statusCode = response.StatusCode;
			var contentString = await response.Content.ReadAsStringAsync(cancellationToken);

			if (statusCode != HttpStatusCode.OK)
			{
				_logger.LogError("Invalid status code {StatusCode} {Content}", statusCode, contentString);
				return YandexSmartCaptchaValidationResultCode.UnknownError;
			}

			var content = JsonNode.Parse(contentString);

			var status = (string)content["status"];
			if (status == "ok")
			{
				return YandexSmartCaptchaValidationResultCode.Success;
			}

			_logger.LogWarning("Validation failed {StatusCode} {Content}", statusCode, content);
			return YandexSmartCaptchaValidationResultCode.ValidationFail;
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Unknown error");
			return YandexSmartCaptchaValidationResultCode.UnknownError;
		}
	}

	[SuppressMessage("Performance", "CA1822:Mark members as static")]
	public string TryGetErrorMessage(YandexSmartCaptchaValidationResultCode code)
	{
		return code switch
		{
			YandexSmartCaptchaValidationResultCode.ValidationFail => "Капча не пройдена",
			YandexSmartCaptchaValidationResultCode.NoSmartToken => "Капча не пройдена",
			YandexSmartCaptchaValidationResultCode.UnknownError => "Ошибка при проверке капчи, попробуйте еще раз",
			_ => null,
		};
	}
}
