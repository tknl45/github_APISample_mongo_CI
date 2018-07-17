using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace APISample.Helper
{
    public class MongoLogger
    {

        static IMongoDatabase _db;

        public static IConfiguration Configuration { get; set; }
        public static MongoLogger getInstance()
		{

            if (_db == null)
			{
                //讀取設定檔
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
                Configuration = builder.Build();


                //連線mongodb
                var connectionString = Configuration["MongoLogConnection:ConnectionString"];
                var database = Configuration["MongoLogConnection:Database"];


                MongoClient _client = new MongoClient(connectionString);
                _db = _client.GetDatabase(database);


            }

			return new MongoLogger();
		}

        public void Log(string message, string TAG="LOG")
        {
            var prjName = Assembly.GetCallingAssembly().GetName().Name;

            MongoLog log = new MongoLog
            {
                message = message,
                logLevel = TAG
            };
            _db.GetCollection<MongoLog>($"{prjName}_log").InsertOne(log);
        }

        public void LogInfo(string message)
        {
            var prjName = Assembly.GetCallingAssembly().GetName().Name;

            MongoLog log = new MongoLog
            {
                message = message,
                logLevel = "INFO"
            };
            _db.GetCollection<MongoLog>($"{prjName}_log").InsertOne(log);
        }


        //寫入錯誤紀錄
        public void LogError(Exception exception)
        {
            var prjName = Assembly.GetCallingAssembly().GetName().Name;
           


            MongoLog log = new MongoLog
            {
                message = exception.Message,
                logLevel = "ERROR",
                stackTrace = exception.StackTrace

            };
            _db.GetCollection<MongoLog>($"{prjName}_log").InsertOne(log);
        }

        //讀取錯誤紀錄
        public List<MongoLog> ListLog()
        {
            var prjName = Assembly.GetCallingAssembly().GetName().Name;

            var list =  _db.GetCollection<MongoLog>($"{prjName}_log")
            .Find(Builders<MongoLog>.Filter.Empty)
            .ToList();

            return list;
        }
    }



    /// <summary>
    /// 紀錄物件
    /// </summary>
    public class MongoLog
    {
        [JsonIgnore]
        public ObjectId Id { get; set; }

        public DateTime updatetime { get; set; } = DateTime.Now;
        public String message { get; set; }

        public String stackTrace { get; set; }

        public string logLevel { get; set; }
        
    }
}