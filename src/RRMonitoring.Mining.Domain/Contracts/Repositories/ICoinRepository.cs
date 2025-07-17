using Nomium.Core.Data.Repositories;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Domain.Contracts.Repositories;

public interface ICoinRepository : IDictionaryRepository<Coin, byte>;
