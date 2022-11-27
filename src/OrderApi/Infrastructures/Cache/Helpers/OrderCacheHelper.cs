namespace Order.Infrastructures.Cache.Helpers;

public static partial class CacheHelper
{
	public static string GetUserOrderKey(long userId) => $"order:{userId}";
}
