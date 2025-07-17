using System;
using System.IO;
using Microsoft.Extensions.Options;
using RRMonitoring.Equipment.Application.Configuration;

namespace RRMonitoring.Equipment.Application.Features.Firmwares;

public class FirmwareUrlResolver(IOptions<EquipmentS3FileProviderConfiguration> options)
{
	public string GetFilePathById(Guid id, string originFileName)
	{
		return $"{options.Value.FirmwareFolder}/{id}{Path.GetExtension(originFileName)}";
	}
}
