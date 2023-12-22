﻿using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RatesConverter.Models
{
	public class CacheService : ICacheService
	{
        private IDatabase? _database;

        public CacheService()
        {
            _database = ConnectionHelper.Connection.GetDatabase();
        }

        public T GetData<T>(string key)
        {
            var value = _database.StringGet(key);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return default;
        }

        public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            var isSet = _database.StringSet(key, JsonConvert.SerializeObject(value), expiryTime);

            return isSet;
        }

        public object RemoveData(string key)
        {
            bool _isKeyExist = _database.KeyExists(key);

            if (_isKeyExist == true)
            {
                return _database.KeyDelete(key);
            }

            return false;
        }
    }
}

