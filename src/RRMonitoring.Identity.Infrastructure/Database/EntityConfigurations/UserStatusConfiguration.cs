using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Identity.Domain.Entities;

namespace RRMonitoring.Identity.Infrastructure.Database.EntityConfigurations;

public class UserStatusConfiguration : IEntityTypeConfiguration<UserStatus>
{
	public void Configure(EntityTypeBuilder<UserStatus> builder)
	{
		builder
			.ToTable("user_status");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.HasData(GetDefaultUserStatuses());
	}

	private static IList<UserStatus> GetDefaultUserStatuses()
	{
		return new List<UserStatus>
		{
			new(1, "Запрос на подключение"),
			new(2, "Активный"),
			new(3, "Заблокирован"),
			new(4, "Удалённый")
		};
	}
}
