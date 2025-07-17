using System.Collections.Generic;

namespace RRMonitoring.Equipment.Scanner.BusEvents.Models;

public record DeviceInfo
{
	public string Ip { get; init; } = null!;
	public string Vendor { get; set; } = "";
	public string Model { get; set; } = "";
	public string Firmware { get; set; } = "";
	public string ApiType { get; set; } = "";
	public List<int> OpenPorts { get; } = [];

	public DeviceInfo(string ip) => Ip = ip;
}
