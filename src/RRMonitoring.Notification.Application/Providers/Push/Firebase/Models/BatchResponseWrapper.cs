using System;
using System.Collections.Generic;
using System.Net.Http;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;

namespace RRMonitoring.Notification.Application.Providers.Push.Firebase.Models;

public class BatchResponseWrapper
{
	public IReadOnlyList<SendResponseWrapper> Responses { get; set; }

	public int SuccessCount { get; set; }

	public int FailureCount => Responses.Count - SuccessCount;
}

public class SendResponseWrapper
{
	public string MessageId { get; set; }

	public FirebaseMessagingExceptionWrapper Exception { get; set; }

	public bool IsSuccess => !string.IsNullOrEmpty(MessageId);
}

public class FirebaseMessagingExceptionWrapper(
	ErrorCode code,
	string message,
	MessagingErrorCode? fcmCode = null,
	Exception inner = null,
	HttpResponseMessage response = null)
	: Exception(message, inner)
{
	public MessagingErrorCode? MessagingErrorCode { get; private set; } = fcmCode;

	public ErrorCode ErrorCode { get; private set; } = code;

	public HttpResponseMessage HttpResponse { get; private set; } = response;
}
