using RedLockNet.SERedis;

namespace Order.Infrastructures.Cache.Redis;

public interface IRedLockService
{
	RedLockFactory GetRedLockFactory();
}
