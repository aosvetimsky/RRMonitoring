using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Application.Features.Pools.Get;

public class GetPoolByIdRequest : BaseRequest<Guid, PoolByIdResponseDto>;

public class GetPoolByIdHandler(IPoolRepository poolRepository) : BaseRequestHandler<GetPoolByIdRequest, Guid, PoolByIdResponseDto>
{
	protected override async Task<PoolByIdResponseDto> HandleData(Guid requestData, CancellationToken cancellationToken)
	{
		var includePaths = new[]
		{
			nameof(Pool.CoinAddresses)
		};

		var pool = await poolRepository.GetById(requestData, includePaths: includePaths, asNoTracking: true, cancellationToken: cancellationToken);
		if (pool is null)
		{
			throw new ValidationException($"{nameof(Pool)} with id {requestData} is not found");
		}

		return new PoolByIdResponseDto
		{
			Id = pool.Id,
			Name = pool.Name,
			CoinAddresses = pool.CoinAddresses
				.Select(ca => new PoolCoinAddressResponseDto
				{
					CoinId = ca.CoinId,
					FirstAddress = ca.FirstAddress,
					SecondAddress = ca.SecondAddress,
					ThirdAddress = ca.ThirdAddress
				})
				.ToList()
		};
	}
}
