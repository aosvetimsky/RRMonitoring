using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class RackConfiguration : IEntityTypeConfiguration<Rack>
{
	public void Configure(EntityTypeBuilder<Rack> builder)
	{
		builder.ToTable("rack");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasOne(x => x.Container)
			.WithMany(x => x.Racks)
			.HasForeignKey(x => x.ContainerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.SocketType)
			.WithMany()
			.HasForeignKey(x => x.SocketTypeId)
			.OnDelete(DeleteBehavior.Restrict);

	}
}
