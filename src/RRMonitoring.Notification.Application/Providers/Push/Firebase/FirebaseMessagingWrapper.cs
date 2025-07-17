using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Options;
using RRMonitoring.Notification.Application.Configuration.Providers;
using RRMonitoring.Notification.Application.Providers.Push.Firebase.Models;

namespace RRMonitoring.Notification.Application.Providers.Push.Firebase;

public class FirebaseMessagingWrapper : IFirebaseMessagingWrapper
{
	private readonly FirebaseMessaging _firebaseMessaging;
	private readonly IMapper _mapper;

	public FirebaseMessagingWrapper(
		IOptions<FirebasePushProviderConfiguration> configuration,
		IMapper mapper)
	{
		FirebaseApp.Create(new AppOptions
		{
			Credential = GoogleCredential.FromJson(JsonSerializer.Serialize(configuration.Value.PrivateKey))
		});

		_firebaseMessaging = FirebaseMessaging.DefaultInstance;

		_mapper = mapper;
	}

	public async Task<BatchResponseWrapper> SendMulticastAsync(MulticastMessage message)
	{
		var batchResponse = await _firebaseMessaging.SendMulticastAsync(message);

		return _mapper.Map<BatchResponseWrapper>(batchResponse);
	}
}
