using System;
using System.Threading;
using System.Threading.Tasks;

namespace RRMonitoring.Identity.Domain.Contracts;

public interface ITransactionScopeManager
{
	Task Execute(Func<Task> action, CancellationToken cancellationToken = default);
}
