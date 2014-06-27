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
        private readonly Logger _logger;

        public DailyTextManager(IDailyTextManagerQuery dailyTextManagerQuery)
        {
            _dailyTextManagerQuery = dailyTextManagerQuery;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public DailyText GetTodaysText()
        {
            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);
            
            //Next Check SQL Server
            var dailyText = _dailyTextManagerQuery.GetDailyText(easternTime);
            if (dailyText != null)
            {
                _logger.Info(string.Format("Daily Text Found in SQL Server with date: {0}", easternTime));

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

            _logger.Info(string.Format("Daily Text retrieved from WOL with date: {0}", easternTime));
            
            //Put the text into SQL Server
            
            _dailyTextManagerQuery.InsertDailyText(dailyText);

            return dailyText;
        }

        public DailyText GetText(Guid id)
        {
            return _dailyTextManagerQuery.GetDailyText(id);
        }

        public List<DailyText> GetDailyTextList(int maxItems = 10)
        {
            return _dailyTextManagerQuery.GetDailyTextList(maxItems);
        }

        private static string ParseNode(HtmlDocument doc, string xPath)
        {
            return doc.DocumentNode.SelectSingleNode(xPath).InnerText ?? string.Empty;
        }

        public void Dispose()
        {
            _dailyTextManagerQuery.Dispose();
        }
    }
}