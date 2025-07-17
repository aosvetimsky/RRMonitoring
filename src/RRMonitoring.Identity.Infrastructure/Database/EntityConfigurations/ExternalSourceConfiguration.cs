using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class ExternalSourceConfiguration : IEntityTypeConfiguration<ExternalSource>
{
	public void Configure(EntityTypeBuilder<ExternalSource> builder)
	{
		builder.ToTable("external_source");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder
			.Property(x => x.Code)
			.IsRequired()
			.HasMaxLength(50);

		builder
			.HasIndex(x => x.Code)
			.IsUnique();

		builder.HasData(GetDefaultSources());
	}

	private static IEnumerable<ExternalSource> GetDefaultSources()
	{
		return new List<ExternalSource>
		{
			new(1, "Azure AD", "azure_ad")
		};
	}
}
