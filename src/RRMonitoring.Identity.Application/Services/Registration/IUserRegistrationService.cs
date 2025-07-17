using System;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.Registration;

public interface IUserRegistrationService
{
	Task<int> GetRegistrationLinkTimeout(Guid userId);

	Task SendLink(User user, string link);
}
