using Nomium.Core.Data.EntityFrameworkCore.Repositories;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.Repositories;

internal class CoinRepository(MiningContext context)
  : DictionaryRepository<Coin, byte>(context), ICoinRepository
{
}
