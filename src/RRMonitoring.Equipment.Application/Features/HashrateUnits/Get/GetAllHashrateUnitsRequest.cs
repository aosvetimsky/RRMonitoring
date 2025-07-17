using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using RRMonitoring.Equipment.Domain.Contracts.Repositories;
using RRMonitoring.Equipment.PublicModels.HashrateUnits;

namespace RRMonitoring.Equipment.Application.Features.HashrateUnits.Get;

public class GetAllHashrateUnitsRequest : IRequest<IReadOnlyList<HashrateUnitResponseDto>>;

public class GetAllHashrateUnitsHandler(IHashrateUnitRepository hashrateUnitRepository, IMapper mapper) : IRequestHandler<GetAllHashrateUnitsRequest, IReadOnlyList<HashrateUnitResponseDto>>
{
	public async Task<IReadOnlyList<HashrateUnitResponseDto>> Handle(GetAllHashrateUnitsRequest request, CancellationToken cancellationToken)
	{
		var coins = await hashrateUnitRepository.GetAll(cancellationToken: cancellationToken);

		return mapper.Map<IReadOnlyList<HashrateUnitResponseDto>>(coins);
	}
}
