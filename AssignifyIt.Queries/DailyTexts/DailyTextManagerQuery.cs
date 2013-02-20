using System;
using System.Collections.Generic;
using System.Linq;
using AssignifyIt.Models;
using PetaPoco;

namespace AssignifyIt.Queries.DailyTexts
{
    public interface IDailyTextManagerQuery
    {
        DailyText GetDailyText(DateTime date);
        DailyText GetDailyText(Guid id);
        List<DailyText> GetDailyTextList(int maxItems);
        void InsertDailyText(DailyText dailyText);
    }


    public class DailyTextManagerQuery : IDailyTextManagerQuery
    {
        private readonly Database _db;

        public DailyTextManagerQuery(string connectionString)
        {
            _db = new Database(connectionString, "System.Data.SqlClient");
        }

        public DailyText GetDailyText(DateTime date)
        {
            return
                _db.Query<DailyText>(
                    string.Format("SELECT * FROM dbo.DailyText WHERE CONVERT(DATE,DateEntered) = CONVERT(DATE,{0})",
                                  "'" + date + "'")).FirstOrDefault();
        }

        public DailyText GetDailyText(Guid id)
        {
            return _db.Query<DailyText>(string.Format("SELECT * FROM dbo.DailyText WHERE Id = {0}", "'" +id+"'")).FirstOrDefault();
        }

        public List<DailyText> GetDailyTextList(int maxItems)
        {
            return
                _db.Query<DailyText>(string.Format("SELECT TOP {0} * FROM dbo.DailyText ORDER BY DateEntered DESC",
                                                   maxItems)).ToList();
        }

        public void InsertDailyText(DailyText dailyText)
        {
            _db.Insert(dailyText);
        }

    }
}