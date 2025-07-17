using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using RRMonitoring.Notification.Application.Providers.Push.Firebase.Models;

namespace RRMonitoring.Notification.Application.Providers.Push.Firebase;

public interface IFirebaseMessagingWrapper
{
	Task<BatchResponseWrapper> SendMulticastAsync(MulticastMessage message);
}
