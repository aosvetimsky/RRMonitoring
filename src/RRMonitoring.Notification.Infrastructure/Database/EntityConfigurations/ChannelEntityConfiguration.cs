using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Notification.Domain.Entities;

namespace RRMonitoring.Notification.Infrastructure.Database.EntityConfigurations;

public class ChannelEntityConfiguration : IEntityTypeConfiguration<Channel>
{
	public void Configure(EntityTypeBuilder<Channel> builder)
	{
		builder.ToTable("channel");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(250);

		builder.HasData(GetDefaultChannels());
	}

	private static IList<Channel> GetDefaultChannels()
	{
		return new List<Channel>
		{
			new() { Id = 1, Name = "Email" },
			new() { Id = 2, Name = "Push" },
			new() { Id = 3, Name = "Sms" },
		};
	}
}
