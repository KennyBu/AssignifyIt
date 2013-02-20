using System;
using System.Collections.Generic;
using System.Globalization;
using AssignifyIt.Models;
using AssignifyIt.Queries.DailyTexts;
using HtmlAgilityPack;

namespace AssignifyIt.Managers
{
    public interface IDailyTextManager
    {
        DailyText GetTodaysText();
        DailyText GetText(Guid id);
        List<DailyText> GetDailyTextList(int maxItems = 10);
    }
    
    public class DailyTextManager : IDailyTextManager
    {
        private readonly IDailyTextManagerQuery _dailyTextManagerQuery;

        public DailyTextManager(IDailyTextManagerQuery dailyTextManagerQuery)
        {
            _dailyTextManagerQuery = dailyTextManagerQuery;
        }

        public DailyText GetTodaysText()
        {

            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone);

            var dailyText = _dailyTextManagerQuery.GetDailyText(easternTime);
            if (dailyText != null)
                return dailyText;
            
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
    }
}