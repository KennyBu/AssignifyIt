using System;
using AssignifyIt.Models;
using ServiceStack.Redis;

namespace AssignifyIt.Managers
{
    public interface IRedisManager : IDisposable
    {
        DailyText GetDailyText(DateTime date);
        void InsertDailyText(DailyText dailyText);
    }

    public class RedisManager : IRedisManager
    {
        private readonly RedisClient _redisClient;
        
        public RedisManager(string url)
        {
            var connectionUri = new Uri(url);
            _redisClient = new RedisClient(connectionUri);
        }
        
        public DailyText GetDailyText(DateTime date)
        {
            var dailyText = _redisClient.Get<DailyText>(date.ToLongDateString());

            return dailyText ;
        }

        public void InsertDailyText(DailyText dailyText)
        {
            _redisClient.Set(dailyText.DateEntered.ToLongDateString(), dailyText);
        }

        public void Dispose()
        {
            _redisClient.Dispose();
        }
    }
}