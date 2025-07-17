using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nomium.Core.Exceptions;
using Nomium.Core.MediatR;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.PublicModels.Pools;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Application.Features.Pools.Create;

public class CreatePoolRequest : BaseRequest<CreatePoolRequestDto, Guid>;

public class CreatePoolRequestHandler(IPoolRepository poolRepository, ICoinRepository coinRepository) : BaseRequestHandler<CreatePoolRequest, CreatePoolRequestDto, Guid>
{
	protected override async Task<Guid> HandleData(CreatePoolRequestDto requestData, CancellationToken cancellationToken)
	{
		var coinAddressesIds = requestData.CoinAddresses
			.Select(ca => ca.CoinId)
			.ToArray();

		var existingCoins = await coinRepository.GetByIds(coinAddressesIds, asNoTracking: true, cancellationToken: cancellationToken);

		var existingCoinIds = existingCoins
			.Select(c => c.Id)
			.ToArray();

		var nonExistingCoins = coinAddressesIds.Except(existingCoinIds).ToList();
		if (nonExistingCoins.Count != 0)
		{
			throw new ValidationException($"{nameof(Coin)} with ids \"{string.Join(',', nonExistingCoins)}\" doesn't exist.");
		}

		var sameNamePool = await poolRepository.GetByName(requestData.Name, cancellationToken: cancellationToken);
		if (sameNamePool is not null)
		{
			throw new ValidationException($"{nameof(Pool)} with name \"{requestData.Name}\" already exists.");
		}

		var poolToAdd = new Pool
		{
			Name = requestData.Name,
			CoinAddresses = requestData.CoinAddresses
				.Select(ca => new PoolCoinAddress
				{
					CoinId = ca.CoinId,
					FirstAddress = ca.FirstAddress,
					SecondAddress = ca.SecondAddress,
					ThirdAddress = ca.ThirdAddress
				})
				.ToArray()
		};

		await poolRepository.Add(poolToAdd, cancellationToken);

		return poolToAdd.Id;
	}
}
