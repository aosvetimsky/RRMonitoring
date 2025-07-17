using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class FacilityTechnicianConfiguration : IEntityTypeConfiguration<FacilityTechnician>
{
	public void Configure(EntityTypeBuilder<FacilityTechnician> builder)
	{
		builder.ToTable("facility_technician");

		builder.HasKey(x => new { x.UserId, x.FacilityId });

		builder
			.HasOne(x => x.Facility)
			.WithMany(x => x.Technicians)
			.HasForeignKey(x => x.FacilityId);
	}
}
