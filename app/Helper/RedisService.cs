using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using System.Threading;

using StackExchange.Redis;

namespace APISample.Helper
{
	public class RedisService
	{
        static object _lock = new object();
		

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection;


        public static IConfiguration Configuration { get; set; }

        static RedisService(){
           
            //讀取設定檔
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            ConfigurationOptions option = new ConfigurationOptions
            {
                AbortOnConnectFail = false,
                EndPoints = { Configuration["RedisConnection:ConnectionString"] }
            };
            
            
            //連線
            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(option));
        }   

        public static ConnectionMultiplexer Connection => LazyConnection.Value;    
        public static IDatabase GetDB => Connection.GetDatabase();
        public static IServer GetServer => Connection.GetServer(Configuration["RedisConnection:ConnectionString"]);
		
        public static RedisService getInstance()
		{
            return new RedisService();
		}

        /// <summary>
        /// 取得值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
		public async Task<T> GetAsync<T>(string key){
			var value = await GetDB.StringGetAsync(key);

			if (!value.IsNull)
				return JsonConvert.DeserializeObject<T>(value);
			else
			{
				return default(T);
			}
		}
		
        /// <summary>
        /// 設定值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
		public async Task<bool> SetAsync(string key, object value)
			 => await GetDB.StringSetAsync(key, JsonConvert.SerializeObject(value), TimeSpan.FromHours(6));


        /// <summary>
        /// 移除key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key){
            GetDB.KeyDelete(key);
        }


        /// <summary>
        /// 移除全部key
        /// </summary>
        public void RemoveAll(){
            foreach (var key in GetServer.Keys())
            {
                GetDB.KeyDelete(key);
            }
        }


        public List<string> GetKeys()
        {
            List<string> list = new List<string>();
            foreach (var key in GetServer.Keys())
            {
                list.Add(key);
            }

            return list;
        }


    }
}