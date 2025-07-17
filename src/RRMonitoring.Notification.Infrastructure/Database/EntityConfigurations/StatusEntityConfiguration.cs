using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class StatusEntityConfiguration : IEntityTypeConfiguration<Status>
{
	public void Configure(EntityTypeBuilder<Status> builder)
	{
		builder.ToTable("status");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(250);

		builder.HasData(GetDefaultStatuses());
	}

	private static IList<Status> GetDefaultStatuses()
	{
		return new List<Status>
		{
			new() { Id = 1, Name = "queued" },
			new() { Id = 2, Name = "delivered" },
			new() { Id = 3, Name = "failed" },
		};
	}
}
