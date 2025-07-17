using System;
using System.Threading;
using System.Threading.Tasks;
using RRMonitoring.Identity.Domain.Contracts;
using RRMonitoring.Identity.Infrastructure.Database;

namespace RRMonitoring.Identity.Infrastructure.Transactions;

internal class TransactionScopeManager : ITransactionScopeManager
{
	private readonly IdentityContext _context;

	public TransactionScopeManager(IdentityContext context)
	{
		_context = context;
	}

	public async Task Execute(Func<Task> action, CancellationToken cancellationToken)
	{
		using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

		try
		{
			await action();
			await _context.SaveChangesAsync(cancellationToken);
			await transaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await transaction.RollbackAsync(cancellationToken);
			throw;
		}
	}
}
