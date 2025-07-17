using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

internal class SigningKeysConfiguration : IEntityTypeConfiguration<SigningKey>
{
	public void Configure(EntityTypeBuilder<SigningKey> builder)
	{
		builder.ToTable("signing_key");

		builder.Property(x => x.Value)
			.HasMaxLength(2048)
			.IsRequired();

		builder.Property(x => x.CreationDate)
			.HasDefaultValueSql("now()");
	}
}
