using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Models;
using RRMonitoring.Bff.Admin.Application.Services.Equipment.Models;

namespace RRMonitoring.Bff.Admin.Application.Services.Equipment;

public interface IEquipmentService
{
	Task<PagedList<SearchEquipmentManufacturersResponse>> SearchManufacturers(SearchEquipmentManufacturersRequest request, CancellationToken cancellationToken);
}
