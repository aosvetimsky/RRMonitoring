using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
	public void Configure(EntityTypeBuilder<Client> builder)
	{
		builder.ToTable("client");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.ExternalId)
			.IsRequired();

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();

		builder
			.HasMany(x => x.Workers)
			.WithOne(x => x.Client)
			.HasForeignKey(x => x.ClientId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}
