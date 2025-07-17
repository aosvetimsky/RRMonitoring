using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
	public void Configure(EntityTypeBuilder<Facility> builder)
	{
		builder.ToTable("facility");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasMany(x => x.Containers)
			.WithOne(x => x.Facility)
			.HasForeignKey(x => x.FacilityId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
