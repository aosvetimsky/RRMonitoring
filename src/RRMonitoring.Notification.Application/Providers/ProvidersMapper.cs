using AutoMapper;
using FirebaseAdmin.Messaging;
using RRMonitoring.Notification.Application.Providers.Push.Firebase.Models;

namespace RRMonitoring.Notification.Application.Providers;

public class ProvidersMapper : Profile
{
	public ProvidersMapper()
	{
		CreateMap<SendResponse, SendResponseWrapper>()
			.ForMember(dest => dest.Exception, opt =>
			{
				opt.PreCondition(src => src.Exception != null);
				opt.MapFrom(src =>
					new FirebaseMessagingExceptionWrapper(
						src.Exception.ErrorCode,
						src.Exception.Message,
						src.Exception.MessagingErrorCode,
						src.Exception.InnerException,
						src.Exception.HttpResponse));
			});

		CreateMap<BatchResponse, BatchResponseWrapper>();
	}
}
