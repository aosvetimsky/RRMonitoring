using System;
using System.Threading;
using System.Threading.Tasks;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts;

internal interface ITransactionalSqlQueryExecutor
{
	delegate Task ExecuteSqlQueryDelagate(string sqlQuery, CancellationToken cancellationToken);

	Task ExecuteInTransaction(Func<ExecuteSqlQueryDelagate, CancellationToken, Task> operation, CancellationToken cancellationToken);
}
