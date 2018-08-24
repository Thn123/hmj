using Hmj.Entity;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class WXResourcereRepository : BaseRepository
    {
        //
        public List<ORG_STORE> GetStores(int orgId)
        {
            string sql = "select * from dbo.ORG_STORE where org_Id=@orgId and (ShowOnWechat=1 or ShowOnWechat is null)";
            return base.Query<ORG_STORE>(sql, new { orgId = orgId });
        }

        public List<PROD_CATEGORY> GetServices(int orgId, Guid parentID, int ptype)
        {
            string sql = @"select *,0 type,'' fname from PROD_CATEGORY where
PARENT_ID=@PARENT_ID and PROD_TYPE=@PROD_TYPE  AND ORG_ID=@ORG_ID";
            return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = ptype, PARENT_ID = parentID, ORG_ID = orgId });
        }


        public List<ORG_EMPLOYEE> GetFreeEmployees(int storId, DateTime beginTime, DateTime endTime, bool? gender)
        {
            string strGender = string.Empty;
            string sql = @"SELECT oe.ID,oe.NAME,op.POST_NAME,op.POST_TYPE 
FROM [ORG_EMPLOYEE] oe LEFT JOIN [ORG_POST] op ON oe.POST_ID =op.ID 
WHERE oe.STORE_ID=@STORE_ID AND oe.STATUS=1";// AND op.POST_TYPE='P0001'";
            if (gender.HasValue)
            {
                strGender = (gender.Value == false) ? "女" : "男";
                sql += @" AND GENDER=@GENDER";
            }
            //            sql += @" and oe.ID NOT in (SELECT [STAFF_ID] FROM CUST_BOOKING where STORE_ID=@STORE_ID and STATUS IN (0,1,2) and staff_id is not null and 
            //                ((BEGIN_DATE>=@BEGIN_Time and BEGIN_DATE<=@END_TIME) or(END_DATE>=@BEGIN_Time and END_DATE<=@END_TIME)))";

            string conflictSql = @"SELECT [STAFF_ID] FROM [CUST_BOOKING] WHERE [STORE_ID]=@STORE_ID AND [STATUS] IN (0,1,2) AND [STAFF_ID] IS NOT NULL AND 
(([BEGIN_DATE]>=@BEGIN_Time AND [BEGIN_DATE]<=@END_TIME) OR([END_DATE]>=@BEGIN_Time AND [END_DATE]<=@END_TIME))";

            string scheduleSql = @"SELECT [EMPLOYEE_ID] FROM (SELECT [ID],[ORG_ID],[EMPLOYEE_ID],[STORE_ID],[SCHEDULE_DATE]
,convert(datetime,(convert(varchar(100),[SCHEDULE_DATE],111) +' '+ convert(varchar(100),[BEGIN_TIME],20)),111) as [SCHEDULE_BEGIN_TIME]
,convert(datetime,(convert(varchar(100),[SCHEDULE_DATE],111) +' '+ convert(varchar(100),[END_TIME],20)),111) as [SCHEDULE_END_TIME] 
FROM [EMPLOYEE_SCHEDULE] 
WHERE [SCHEDULE_DATE]=convert(date,@BEGIN_Time) AND [STORE_ID]=@STORE_ID) ExpansionT 
WHERE [SCHEDULE_BEGIN_TIME]<=@BEGIN_Time AND [SCHEDULE_END_TIME]>=@END_TIME";

            sql += string.Format(@"AND oe.ID NOT IN ({0}) AND oe.ID IN ({1})", conflictSql, scheduleSql);

            return base.Query<ORG_EMPLOYEE>(sql, new { STORE_ID = storId, BEGIN_Time = beginTime, END_TIME = endTime, GENDER = strGender });
        }
    }
}
