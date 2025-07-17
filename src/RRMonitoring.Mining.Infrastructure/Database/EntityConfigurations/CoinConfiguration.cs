using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRMonitoring.Mining.Domain.Entities;

namespace RRMonitoring.Mining.Infrastructure.Database.EntityConfigurations;

public class CoinConfiguration : IEntityTypeConfiguration<Coin>
{
	public void Configure(EntityTypeBuilder<Coin> builder)
	{
		builder.ToTable("coin");

		builder.HasKey(x => x.Id);

		builder
			.Property(x => x.Name)
			.IsRequired();

		builder
			.Property(x => x.Ticker)
			.IsRequired();

		builder
			.Property(x => x.ExternalId)
			.IsRequired();

		builder.HasIndex(x => x.Ticker)
			.IsUnique();

		builder.HasIndex(x => x.ExternalId)
			.IsUnique();

		builder.HasData(GetDefaultCoins());
	}

	private static IReadOnlyList<Coin> GetDefaultCoins()
	{
		return [
			new Coin(1, "Conflux", "CFX", "89bb611e-7a3c-4adf-a190-89ceaaa6b174"),
			new Coin(2, "EthereumPoW", "ETHW", "728912a7-1a00-4abb-96c0-fba5d4123957"),
			new Coin(3, "EthereumFair", "ETHF", "049a7e90-f063-4532-acd7-6fc74ec1ec64"),
			new Coin(4, "Nervos Network", "CKB", "7f08ec67-64f4-40ce-b18f-3a659d6614a8"),
			new Coin(5, "Ergo", "ERG", "7d760102-9094-4829-a65a-ba749bddf013"),
			new Coin(6, "Handshake", "HNS", "2bfc15a8-a452-4c03-9f7a-80b8ed853415"),
			new Coin(7, "Qitmeer Network", "MEER", "975bc446-5f0e-4349-b00a-4a1d5f7b51a3"),
			new Coin(8, "eCash", "XEC", "c440283b-04c4-4f7b-b622-debe5b867c95"),
			new Coin(9, "Bitcoin", "BTC", "f28930db-d005-43b3-8ffd-5b62b15785df"),
			new Coin(10, "Ethereum Classic", "ETC", "223e7ec7-c749-4909-b56f-cf2bee5aaa85"),
			new Coin(11, "Kaspa", "KAS", "86961652-cb82-4042-b3ae-05131494535b"),
			new Coin(12, "Dash", "DASH", "1326ccaa-5082-4304-9270-0ce4e81fb57d"),
			new Coin(13, "Litecoin", "LTC", "d175f805-b52f-4b93-b568-174bc6fca57d"),
			new Coin(14, "Zcash", "ZEC", "b5bd1ee1-748e-481f-b4bf-f621989113db"),
			new Coin(15, "Horizen", "ZEN", "3c96d353-adf6-4690-8f93-f85aa173441e"),
			new Coin(16, "Aleo", "ALEO", "8f80498f-54ef-4b9d-957a-3b6ba4a26df3"),
			new Coin(17, "Ravencoin", "RVN", "cff35eb9-ed0c-4c5f-8c3f-68454bba3343"),
			new Coin(18, "Kadena", "KDA", "25f59887-83d4-463b-8ec3-18a42abad96a"),
			new Coin(19, "ScPrime", "SCP", "5651424e-5055-4989-bfc3-062e717d3bff")
			];
	}
}
