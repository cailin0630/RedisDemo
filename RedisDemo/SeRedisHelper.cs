using Newtonsoft.Json;
using StackExchange.Redis;
using StandardResult4Net;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace RedisDemo
{
    /// <summary>
    /// Redis操作类
    /// </summary>
    public class SeRedisHelper
    {
        private static readonly string Conn = ConfigurationManager.AppSettings["SERedis"] ?? "127.0.0.1:6379";

        #region string类型

        public static Result<string> StringGet(string key, int db = -1)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var value = client.GetDatabase(db).StringGet(key);
                    return Result<string>.Success(value);
                }
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }

        public static Result<string[]> StringGetMany(string[] keyStrs, int db = -1)
        {
            try
            {
                var count = keyStrs.Length;
                var keys = new RedisKey[count];
                var addrs = new string[count];

                for (var i = 0; i < count; i++)
                {
                    keys[i] = keyStrs[i];
                }

                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var values = client.GetDatabase(db).StringGet(keys);
                    for (var i = 0; i < values.Length; i++)
                    {
                        addrs[i] = values[i];
                    }
                    return Result<string[]>.Success(addrs);
                }
            }
            catch (Exception ex)
            {
                return Result<string[]>.Fail(ex.Message);
            }
        }

        public static Result StringSet(string key, string value, int db = -1)
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var ret = client.GetDatabase(db).StringSet(key, value);
                    return ret ? Result.Success() : Result.Fail("fail");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public static Result StringSetMany(string[] keysStr, string[] valuesStr, int db = -1)
        {
            try
            {
                var count = keysStr.Length;
                var keyValuePair = new KeyValuePair<RedisKey, RedisValue>[count];
                for (var i = 0; i < count; i++)
                {
                    keyValuePair[i] = new KeyValuePair<RedisKey, RedisValue>(keysStr[i], valuesStr[i]);
                }
                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var ret = client.GetDatabase(db).StringSet(keyValuePair);
                    return ret ? Result.Success() : Result.Fail("fail");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        #endregion string类型

        #region 泛型

        public static Result EntitySet<T>(string key, T t, TimeSpan? ts=null, int db = -1)
        {
            try
            {
                var str = JsonConvert.SerializeObject(t);
                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var ret = client.GetDatabase(db).StringSet(key, str, ts);
                    return ret ? Result.Success() : Result.Fail("fail");
                }
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public static Result<T> EntityGet<T>(string key, int db = -1)
            where T : class
        {
            try
            {
                using (var client = ConnectionMultiplexer.Connect(Conn))
                {
                    var strValue = client.GetDatabase(db).StringGet(key);
                    return string.IsNullOrEmpty(strValue) ? Result<T>.Fail("redis return null") : Result<T>.Success(JsonConvert.DeserializeObject<T>(strValue));
                }
            }
            catch (Exception ex)
            {
                return Result<T>.Fail(ex.Message);
            }
        }

        #endregion 泛型
    }
}