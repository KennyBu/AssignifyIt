using System;
using System.Collections.Generic;
using AssignifyIt.Models;
using AssignifyIt.Queries.DailyTexts;
using HtmlAgilityPack;

namespace AssignifyIt.Managers
{
    public interface IDailyTextManager
    {
        DailyText GetTodaysText();
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
            var dailyText = _dailyTextManagerQuery.GetDailyText(DateTime.UtcNow);
            if (dailyText != null)
                return dailyText;
            
            const string url = "http://wol.jw.org/en/wol/dt/r1/lp-e";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            dailyText = new DailyText
                {
                    Id = Guid.NewGuid(),
                    DateLine = ParseNode(doc, "//p[@class='ss']"),
                    Header = ParseNode(doc, "//p[@class='sa']"),
                    Body = ParseNode(doc, "//p[@class='sb']"),
                    DateEntered = DateTime.UtcNow
                };

            _dailyTextManagerQuery.InsertDailyText(dailyText);

            return dailyText;
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