using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace OrderApi.Infrastructures.Cache.Redis;

    public class RedisServer
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private string _configurationString;
        private int _currentDatabaseId = 0;

        public RedisServer(IOptions<RedisConfigModel> cacheConfigModel)
        {
            if (cacheConfigModel.Value == null)
                throw new ArgumentNullException(nameof(RedisConfigModel));

            _configurationString = $"{cacheConfigModel.Value.Host}:{cacheConfigModel.Value.Port}";
            _connectionMultiplexer = ConnectionMultiplexer.Connect(_configurationString);
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseId);
        }

        public IDatabase Database => _database;

        public void FlushDatabase()
        {
            _connectionMultiplexer.GetServer(_configurationString).FlushDatabase(_currentDatabaseId);
        }
    }
