using System.IO;

namespace RRMonitoring.Equipment.PublicModels.Firmware;

public class FileResponseDto
{
	public Stream Stream { get; set; }

	public string OriginFileName { get; set; }
}
