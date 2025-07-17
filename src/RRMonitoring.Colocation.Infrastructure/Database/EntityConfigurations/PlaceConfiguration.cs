using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class PlaceConfiguration : IEntityTypeConfiguration<Place>
{
	public void Configure(EntityTypeBuilder<Place> builder)
	{
		builder.ToTable("place");

		builder.HasKey(x => x.Id);

		builder
			.HasOne(x => x.Shelf)
			.WithMany(x => x.Places)
			.HasForeignKey(x => x.ShelfId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.SocketType)
			.WithMany()
			.HasForeignKey(x => x.SocketTypeId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
