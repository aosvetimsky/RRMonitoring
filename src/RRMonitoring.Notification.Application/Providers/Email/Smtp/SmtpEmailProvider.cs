using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Providers.Models;
using RRMonitoring.Notification.Domain.Enums;

namespace RRMonitoring.Notification.Application.Providers.Email.Smtp;

internal class SmtpEmailProvider(
	ISmtpClient smtpClient,
	IOptions<SmtpEmailProviderConfiguration> smtpOptions,
	ILogger<SmtpEmailProvider> logger)
	: IEmailProvider
{
	private readonly SmtpEmailProviderConfiguration _smtpOptions = smtpOptions.Value;

	[SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "By design")]
	public async Task<IList<NotificationResult>> SendNotification(NotificationInfo notificationInfo)
	{
		var emailMessage = GetMessageContent(notificationInfo);

		var notificationResult = new NotificationResult
		{
			Recipient = notificationInfo.Recipient,
			RecipientId = notificationInfo.RecipientId,
			NotificationId = notificationInfo.NotificationId,
			Body = notificationInfo.Body,
			ExternalMessageId = emailMessage.MessageId,
			ChannelId = (byte)Channels.Email
		};

		logger.LogInformation("Start sending notification by SMTP provider. Notification info: {@NotificationInfo}",
			notificationInfo);

		try
		{
			await smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.UseSsl);
			await smtpClient.AuthenticateAsync(_smtpOptions.SenderEmail, _smtpOptions.Password);

			await smtpClient.SendAsync(emailMessage);

			notificationResult.IsSuccess = true;
			notificationResult.Status = NotificationStatuses.Delivered;

			logger.LogInformation("Notification with info: {@NotificationInfo} was successfully sent by SMTP provider",
				notificationInfo);

			await smtpClient.DisconnectAsync(quit: false);
		}
		// TODO: Maybe special exception catch?
		catch (Exception ex)
		{
			logger.LogError(ex, "Sending notification by SMTP provider with info: {@NotificationInfo} was failed",
				notificationInfo);

			notificationResult.IsSuccess = false;
			notificationResult.Status = NotificationStatuses.Failed;
			notificationResult.Error = ex.Message;
		}

		return new List<NotificationResult> { notificationResult };
	}

	public NotificationResult ReceiveCallback(CallbackInfo _)
	{
		throw new NotImplementedException();
	}

	private MimeMessage GetMessageContent(NotificationInfo notificationInfo)
	{
		var emailMessage = new MimeMessage();

		emailMessage.From.Add(new MailboxAddress("", _smtpOptions.SenderEmail));
		emailMessage.To.Add(new MailboxAddress("", notificationInfo.Recipient));

		emailMessage.Subject = notificationInfo.Subject ?? string.Empty;
		emailMessage.Body = new TextPart(TextFormat.Html) { Text = notificationInfo.Body };

		if (notificationInfo.Attachments != null)
		{
			var multipart = new Multipart("mixed") { emailMessage.Body, };

			foreach (var attachmentFile in notificationInfo.Attachments)
			{
				var attachment = new MimePart(attachmentFile.ContentType)
				{
					Content = new MimeContent(attachmentFile.OpenReadStream()),
					ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
					ContentTransferEncoding = ContentEncoding.Base64,
					FileName = attachmentFile.FileName
				};

				multipart.Add(attachment);
			}

			emailMessage.Body = multipart;
		}

		return emailMessage;
	}
}
