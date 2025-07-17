using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Nomium.Core.Exceptions;
using RRMonitoring.Mining.Domain.Contracts.Repositories;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Application.Features.Pools.Delete;

public record DeletePoolRequest(Guid Id) : IRequest;

public class DeletePoolHandler(IPoolRepository poolRepository) : IRequestHandler<DeletePoolRequest>
{
	public async Task Handle(DeletePoolRequest request, CancellationToken cancellationToken)
	{
		var pool = await poolRepository.GetById(request.Id, asNoTracking: true, cancellationToken: cancellationToken);
		if (pool is null)
		{
			throw new ValidationException($"{nameof(Pool)} with id: {request.Id} is not found.");
		}

		await poolRepository.Delete(pool, cancellationToken);
	}
}
