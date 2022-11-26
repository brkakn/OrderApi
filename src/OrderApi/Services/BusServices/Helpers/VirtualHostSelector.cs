using System.Runtime.InteropServices;

namespace Order.Services.BusServices.Helpers;

public static class VirtualHostSelector
{
	public static string GetVirtualHostByOS(string virtualhost, bool overwrite = false)
	{
#if DEBUG
		virtualhost = Environment.MachineName;
#endif
		if (!overwrite && RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return Environment.MachineName;
		}
		else
		{
			return virtualhost;
		}
	}
}
