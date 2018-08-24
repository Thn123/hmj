using Hmj.Entity.Entities;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class ReportRepository : BaseRepository
    {
        public List<PASSENGER_EX> QueryPassenger(ReportSearch search)
        {
            string sql = @"SELECT SUM(CUST_NUMBER) AS count,CONVERT(varchar(10),Dates,120) Dates
FROM (SELECT CUST_NUMBER, DATEADD(day, DATEDIFF(day, 0, TRANS_DATE), 0) AS Dates
FROM  ORDER_HEAD where (PAY_STATUS=1 or PAY_STATUS=2) ";
            //if (search.store_id > 0)
            //{
            //    sql += " and store_id= " + search.store_id;
            //}
        
            if (!string.IsNullOrEmpty(search.STORE))
            {
                sql += " and store_id= " + search.STORE;
            }
            if (!string.IsNullOrEmpty(search.BEGIN_DATE))
            {
                sql += " and trans_date>= '" + search.BEGIN_DATE + "'";
            }

            if (!string.IsNullOrEmpty(search.BEGIN_DATE))
            {
                sql += " and trans_date<='" + search.END_DATE+"'";
            }
            sql += " and CUST_NUMBER is not null) AS MM GROUP BY Dates";
            return base.Query<PASSENGER_EX>(sql, new { });
        }



     

        public ORG_INFO_EX GetBegAndEndTime(int orgid)
        {
            return Get<ORG_INFO_EX>("SELECT '1900-01-01 '+CONVERT(VARCHAR,ISNULL(STORE_HOURS_BEGIN,'00:00')) STORE_HOURS_BEGIN,'1900-01-01 '+CONVERT(VARCHAR,ISNULL(STORE_HOURS_END,'00:00')) STORE_HOURS_END FROM dbo.ORG_INFO where id=" + orgid, null);
        }


    }
}
