using System;
using AssignifyIt.Models;
using HtmlAgilityPack;

namespace AssignifyIt.Managers
{
    public interface IDailyTextManager
    {
        DailyText GetTodaysText();
    }
    
    public class DailyTextManager : IDailyTextManager
    {
        public DailyText GetTodaysText()
        {
            const string url = "http://wol.jw.org/en/wol/dt/r1/lp-e";

            var web = new HtmlWeb();
            var doc = web.Load(url);

            return new DailyText
                {
                    Id = Guid.NewGuid(),
                    DateLine = ParseNode(doc, "//p[@class='ss']"),
                    Header = ParseNode(doc, "//p[@class='sa']"),
                    Body = ParseNode(doc, "//p[@class='sb']"),
                };
        }

        private static string ParseNode(HtmlDocument doc, string xPath)
        {
            return doc.DocumentNode.SelectSingleNode(xPath).InnerText ?? string.Empty;
        }
    }
}