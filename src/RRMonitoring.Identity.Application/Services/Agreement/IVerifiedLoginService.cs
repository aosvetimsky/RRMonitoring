using System.Threading.Tasks;

namespace RRMonitoring.Identity.Application.Services.Agreement;

public interface IVerifiedLoginService
{
	Task RememberVerifiedLoginAsync(string userId);

	Task<string> GetVerifiedUserIdAsync();

	void ForgetVerifiedLogin();
}
