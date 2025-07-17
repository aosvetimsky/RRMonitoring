using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.UserValidation;

public interface IUserAccessVerificationService
{
	Task<User> VerifyCodeAndRetrieveUser(string verificationCode, CancellationToken cancellationToken);
}
