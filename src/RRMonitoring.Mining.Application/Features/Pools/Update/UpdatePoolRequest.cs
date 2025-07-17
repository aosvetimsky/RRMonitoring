using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Entities;
using RRMonitoring.Mining.PublicModels.Pools;

namespace RRMonitoring.Mining.Application.Features.Pools.Update;

public class UpdatePoolRequest : BaseRequest<UpdatePoolRequestDto, Guid>;

public class UpdatePoolRequestHandler(IPoolRepository poolRepository, ICoinRepository coinRepository) : BaseRequestHandler<UpdatePoolRequest, UpdatePoolRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(UpdatePoolRequestDto requestData, CancellationToken cancellationToken)
	{
		var pool = await poolRepository.GetById(requestData.Id, includePaths: [nameof(Pool.CoinAddresses)], cancellationToken: cancellationToken);
		if (pool is null)
		{
			throw new ValidationException($"{nameof(Pool)} with id: {requestData.Id} is not found.");
		}

		var coinAddressesIds = requestData.CoinAddresses
			.Select(ca => ca.CoinId)
			.ToArray();

		var existingCoins = await coinRepository.GetByIds(coinAddressesIds, asNoTracking: true, cancellationToken: cancellationToken);

		var existingCoinIds = existingCoins
			.Select(c => c.Id)
			.ToArray();

		var nonExistingCoins = coinAddressesIds.Except(existingCoinIds).ToList();
		if (nonExistingCoins.Any())
		{
			throw new ValidationException($"{nameof(Coin)} with ids \"{string.Join(',', nonExistingCoins)}\" doesn't exist.");
		}

		var existingPool = await poolRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);
		if (existingPool is not null && existingPool.Id != requestData.Id)
		{
			throw new ValidationException($"{nameof(Pool)} with name \"{requestData.Name}\" already exists.");
		}

		pool.Name = requestData.Name;
		pool.CoinAddresses = requestData.CoinAddresses
			.Select(ca => new PoolCoinAddress
			{
				CoinId = ca.CoinId,
				FirstAddress = ca.FirstAddress,
				SecondAddress = ca.SecondAddress,
				ThirdAddress = ca.ThirdAddress
			})
			.ToList();

		await poolRepository.Update(pool, cancellationToken);

		return pool.Id;
	}
}
