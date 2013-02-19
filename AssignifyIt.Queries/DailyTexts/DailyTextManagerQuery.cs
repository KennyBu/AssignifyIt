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
            return _db.FirstOrDefault<DailyText>("SELECT * FROM dbo.DailyText WHERE CONVERT(DATE,GETDATE()) = CONVERT(DATE,{0})",date);
        }

        public List<DailyText> GetDailyTextList(int maxItems)
        {
            return _db.Query<DailyText>("SELECT TOP {0} * FROM dbo.DailyText ORDER BY DateAdded DESC", maxItems).ToList();
        }

        public void InsertDailyText(DailyText dailyText)
        {
            _db.Insert(dailyText);
        }
    }
}