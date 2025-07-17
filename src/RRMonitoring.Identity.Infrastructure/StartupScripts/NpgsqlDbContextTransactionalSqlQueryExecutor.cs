using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace RRMonitoring.Identity.Infrastructure.StartupScripts;

internal sealed class NpgsqlDbContextTransactionalSqlQueryExecutor : ITransactionalSqlQueryExecutor
{
	private readonly ILogger _logger;
	private readonly DbContext _dbContext;
	private readonly bool _doCommit;

	public NpgsqlDbContextTransactionalSqlQueryExecutor(
		ILogger logger,
		DbContext dbContext,
		bool doCommit
	)
	{
		_logger = logger;
		_dbContext = dbContext;
		_doCommit = doCommit;
	}

	public async Task ExecuteInTransaction(
		Func<ITransactionalSqlQueryExecutor.ExecuteSqlQueryDelagate, CancellationToken, Task> operation,
		CancellationToken cancellationToken
	)
	{
		var database = _dbContext.Database;

		var dbConnection = database.GetDbConnection();
		if (dbConnection is NpgsqlConnection npgsqlConnection)
		{
			npgsqlConnection.Notice += (sender, e) =>
			{
				var notice = e.Notice;
				_logger.LogWarning("Notice: {Message}", notice.MessageText);
			};
		}
		else
		{
			throw new NotSupportedException(dbConnection.GetType().Name);
		}

		await database.OpenConnectionAsync(cancellationToken);

		try
		{
			await using var transaction = await database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

			var executedSqlQuery = false;

			await operation(ExecuteSqlQuery, cancellationToken);
			async Task ExecuteSqlQuery(string sqlQuery, CancellationToken cancellationToken)
			{
				var normalizedSql = sqlQuery.Replace("{", "{{").Replace("}", "}}");
				await database.ExecuteSqlRawAsync(normalizedSql, cancellationToken);

				executedSqlQuery = true;
			}

			if (!executedSqlQuery)
			{
				_logger.LogWarning("Nothing to commit");
				return;
			}

			if (_doCommit)
			{
				_logger.LogInformation("Commiting");
				await transaction.CommitAsync(cancellationToken);
			}
			else
			{
				_logger.LogWarning("Rolling back transaction");
			}
		}
		finally
		{
			await database.CloseConnectionAsync();
		}
	}
}
