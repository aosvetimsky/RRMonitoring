using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Colocation.Domain.Entities;

namespace RRMonitoring.Colocation.Infrastructure.Database.EntityConfigurations;

public class SocketTypeConfiguration : IEntityTypeConfiguration<SocketType>
{
	public void Configure(EntityTypeBuilder<SocketType> builder)
	{
		builder.ToTable("socket_type");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.Code)
			.IsRequired();

		builder.HasData(GetDefaultTypes());
	}

	private static List<SocketType> GetDefaultTypes()
	{
		return
		[
			new SocketType(1, "C13", "C13"),
			new SocketType(2, "C19", "C19"),
			new SocketType(3, "PDU", "PDU"),
			new SocketType(4, "EuroPDU", "EuroPDU")
		];
	}
}
