using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.SigningKeys.Management;

public interface IKeyManagementService
{
	Task<SigningKey> GetCurrentKey();

	Task<List<SigningKey>> GetAllKeys();
}
