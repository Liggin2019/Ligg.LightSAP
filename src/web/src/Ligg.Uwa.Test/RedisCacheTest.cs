using Ligg.EntityFramework.Entities;
using Ligg.Infrastructure.DataModels;
using Ligg.Uwa.Application.Shared;
using Ligg.Uwa.Application.Utilities;
using Ligg.Uwa.Basis.SCC;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ligg.Uwa.Application.Test
{
    public class RedisCacheTest
    {
        [SetUp]
        public void Init()
        {
            GlobalContext.SystemSetting = new SystemSetting
            {
                DbProvider = "MySql",
                DbConnectionString = "server=localhost;database=liggmwf;user=root;password=db@pass#123;port=3306;pooling=true;max pool size=20;persist security info=True;charset=utf8mb4;",

                CacheProvider = "Redis",
                RedisConnectionString = "127.0.0.1:6379"
            };
        }

        [Test]
        public void TestRedisSimple()
        {
            string key = "key1";
            string value = "value1";
            CacheFactory.Cache.SetCache<string>(key, value);

            Assert.AreEqual(value, CacheFactory.Cache.GetCache<string>(key));
        }

        [Test]
        public void TestRedisComplex()
        {
            string key = "key2";
            TResult<string> value = new TResult<string> { Flag = 1, Data = "测试Redis" };
            CacheFactory.Cache.SetCache<TResult<string>>(key, value);

            var result = CacheFactory.Cache.GetCache<TResult<string>>(key);
            if (result.Flag == 1)
            {
                Assert.Pass(nameof(TestRedisComplex));
            }
            else
            {
                Assert.Fail(nameof(TestRedisComplex));
            }
        }

    }
}