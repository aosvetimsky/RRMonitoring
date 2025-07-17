using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Application.Services.Permissions;

public interface IPermissionValidator
{
	Task<bool> ArePermissionsAvailableForUser(User user, IList<Guid> permissionsIds);
}
