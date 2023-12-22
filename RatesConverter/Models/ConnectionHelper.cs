using System;
using StackExchange.Redis;

namespace RatesConverter.Models
{
	public class ConnectionHelper
	{
        private static Lazy<ConnectionMultiplexer> lazyConnection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }

        static ConnectionHelper()
        {
            lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
            });
        }

    }
}

