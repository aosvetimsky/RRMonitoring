using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Identity.Application.Services.ForgotPassword.Models;

namespace RRMonitoring.Identity.Application.Services.ForgotPassword;

public interface IForgotPasswordService
{
	Task<int> GetForgotPasswordCodeTimeout(string login, CancellationToken cancellationToken);

	Task<ForgotPasswordResponse> ForgotPasswordDirect(ForgotPasswordRequest request, CancellationToken cancellationToken);

	Task<ForgotPasswordVerifyCodeResponse> VerifyCode(ForgotPasswordVerifyCodeRequest request, CancellationToken cancellationToken);

	Task<int> ResendCode(ForgotPasswordResendCodeRequest request, CancellationToken cancellationToken);
}
