using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class ContainerConfiguration : IEntityTypeConfiguration<Container>
{
	public void Configure(EntityTypeBuilder<Container> builder)
	{
		builder.ToTable("container");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasMany(x => x.Racks)
			.WithOne(x => x.Container)
			.HasForeignKey(x => x.ContainerId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.SocketType)
			.WithMany()
			.HasForeignKey(x => x.SocketTypeId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
