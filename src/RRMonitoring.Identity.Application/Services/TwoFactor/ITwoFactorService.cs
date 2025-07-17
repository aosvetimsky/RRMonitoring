using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.TwoFactor;

public interface ITwoFactorService
{
	Task<int> SendCode(User user, bool isViaEmail = false, bool validateTimeout = false);
}
