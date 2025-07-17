using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Creation;

public interface ISigningKeyCreator
{
	Task<SigningKey> CreateAndStore();
}
