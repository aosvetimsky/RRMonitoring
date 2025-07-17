using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class WorkerStatusConfiguration : IEntityTypeConfiguration<WorkerStatus>
{
	public void Configure(EntityTypeBuilder<WorkerStatus> builder)
	{
		builder.ToTable("worker_status");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired();

		builder.HasData(GetDefaultWorkerStatuses());
	}

	private static IReadOnlyList<WorkerStatus> GetDefaultWorkerStatuses()
	{
		return [
			new WorkerStatus(1, "Unrecognized"),
			new WorkerStatus(2, "Active"),
			new WorkerStatus(3, "Disabled")
			];
	}
}
