using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class ShelfConfiguration : IEntityTypeConfiguration<Shelf>
{
	public void Configure(EntityTypeBuilder<Shelf> builder)
	{
		builder.ToTable("shelf");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.HasOne(x => x.Rack)
			.WithMany(x => x.Shelves)
			.HasForeignKey(x => x.RackId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.SocketType)
			.WithMany()
			.HasForeignKey(x => x.SocketTypeId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
