using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class ExternalPermissionConfiguration : IEntityTypeConfiguration<ExternalPermission>
{
	public void Configure(EntityTypeBuilder<ExternalPermission> builder)
	{
		builder.ToTable("external_permission");

		builder.HasKey(x => x.Id);

		builder.HasData(GetDefaultPermissions());
	}

	private static List<ExternalPermission> GetDefaultPermissions()
	{
		return new List<ExternalPermission>
		{
			new(1, "Майнинг", "mining")
		};
	}
}
