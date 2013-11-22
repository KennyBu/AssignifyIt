using System;
using System.Collections.Generic;
using AssignifyIt.Models;
using AssignifyIt.Queries.DailyTexts;
using HtmlAgilityPack;
using NLog;

namespace AssignifyIt.Managers
{
    public interface IDailyTextManager
    {
        DailyText GetTodaysText();
        DailyText GetText(Guid id);
        List<DailyText> GetDailyTextList(int maxItems = 10);
    }

    public class DailyTextManager : IDailyTextManager, IDisposable
    {
        private readonly IDailyTextManagerQuery _dailyTextManagerQuery;
        private readonly IRedisManager _redisManager;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public DailyTextManager(IDailyTextManagerQuery dailyTextManagerQuery, IRedisManager redisManager)
        {
            _dailyTextManagerQuery = dailyTextManagerQuery;
            _redisManager = redisManager;
        }

        public DailyText GetTodaysText()
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            //First Check Redis Cache
            var dailyText = _redisManager.GetDailyText(easternTime);

            if (dailyText != null)
            {
                logger.Info(string.Format("Daily Text Found in Redis with date: {0}", easternTime));
                return dailyText;
            }
                
            
            //Next Check SQL Server
            dailyText = _dailyTextManagerQuery.GetDailyText(easternTime);
            if (dailyText != null)
            {
                logger.Info(string.Format("Daily Text Found in SQL Server with date: {0}", easternTime));
                _redisManager.InsertDailyText(dailyText);
                return dailyText;
            }
            
            //Retrieve the text from the online library
            var url = string.Format("http://wol.jw.org/en/wol/dt/r1/lp-e/{0}/{1}/{2}", easternTime.Year,easternTime.Month,easternTime.Day);
            var web = new HtmlWeb();
            var doc = web.Load(url);

            dailyText = new DailyText
                {
                    Id = Guid.NewGuid(),
                    DateLine = ParseNode(doc, "//p[@class='ss']"),
                    Header = ParseNode(doc, "//p[@class='sa']"),
                    Body = ParseNode(doc, "//p[@class='sb']"),
                    DateEntered = easternTime
                };

            logger.Info(string.Format("Daily Text retrieved from WOL with date: {0}", easternTime));
            //Put the text into the cache & SQL Server
            _redisManager.InsertDailyText(dailyText);
            _dailyTextManagerQuery.InsertDailyText(dailyText);

            return dailyText;
        }

        public DailyText GetText(Guid id)
        {
            return _dailyTextManagerQuery.GetDailyText(id);
        }

        public List<DailyText> GetDailyTextList(int maxItems = 10)
        {
            var dailyText = GetTodaysText();

            return _dailyTextManagerQuery.GetDailyTextList(maxItems);
        }

        private static string ParseNode(HtmlDocument doc, string xPath)
        {
            return doc.DocumentNode.SelectSingleNode(xPath).InnerText ?? string.Empty;
        }

        public void Dispose()
        {
            _dailyTextManagerQuery.Dispose();
            _redisManager.Dispose();
        }
    }
}