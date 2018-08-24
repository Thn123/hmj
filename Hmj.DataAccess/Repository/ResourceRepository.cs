using Hmj.Entity;
using Hmj.Entity.Entities;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class ResourceRepository : BaseRepository
    {
      
        //得到门店信息
        public List<ORG_STORE> GetAvailableStore(int orgId)
        {
            string sql = "SELECT [ID],[NAME]  FROM [ORG_STORE] WHERE [STATUS]='1' and ORG_ID=" + orgId;
            return base.Query<ORG_STORE>(sql, null);
        }
        //得到区域信息
        public List<AreaManage_EX> GetSelArea()
        {
            string sql = "SELECT AreaNo,Name  FROM AreaManage";
            return base.Query<AreaManage_EX>(sql, null);
        }

        //得到模板区域信息
        public List<GROUP_INFO> GetTemplateArea(string parentCode)
        {
            string content = "";
            if (!string.IsNullOrEmpty(parentCode))
            {
                content="where [PARENT_ID] in (" + parentCode + ")";
            }
            string sql = @"select ID,NAME from GROUP_INFO "+content;
            return base.Query<GROUP_INFO>(sql, null);
        }

        //得到模板区域信息
        //public List<TEMPLATE_INFO> GetTemplateList(string parentCode)
        //{
        //    string content = "";
        //    if (!string.IsNullOrEmpty(parentCode))
        //    {
        //        content = "where [PARENT_ID] in (" + parentCode + ")";
        //    }
        //    string sql = @"select ID,TEMPLATE_NAME from TEMPLATE_INFO " + content;
        //    return base.Query<TEMPLATE_INFO>(sql, null);
        //}


        public List<ORG_STORE> GetAvailableStore(string orgId, string pname, int store, int regionid)
        {
            string sql = "SELECT [ID],[NAME],[CITY]  FROM [ORG_STORE] WHERE [STATUS]='1' and ORG_ID=" + orgId;
            if (!string.IsNullOrEmpty(pname))
            {
                sql += " and  REGION_ID='" + pname + "'";
            }
            if (store > 0)
            {
                sql += " and ID=" + store;
            }
            if (regionid > 0)
            {
                sql += " and REGION_ID=" + regionid;
            }
            return base.Query<ORG_STORE>(sql, null);
        }
        //得到地区信息
        public List<ORG_INFO> GetAvailableRegion(int orgId, int store, int regionid)
        {
            //string sql = "select distinct CITY from ORG_STORE WHERE [STATUS]='1' and CITY is  not null and ORG_ID=" + orgId;
            string sql = "select ID,ORG_NO,ORG_NAME from ORG_INFO where 1=1  and PARENT_ID=" + orgId;
            if (regionid > 0)
            {
                sql += " and  ID=" + regionid;
            }
            if (store > 0)
            {
                sql += " and ID=(select REGION_ID from ORG_STORE where ID=" + store + ")";
            }

            return base.Query<ORG_INFO>(sql, null);
        }

        //得到员工信息
        public List<ORG_EMPLOYEE> GetEmployeeByStoreId(string storId)
        {
            string sql = @"SELECT  oe.ID,ltrim(rtrim(oe.EMPLOYEE_NO)) AS EMPLOYEE_NO,oe.NAME,op.POST_NAME,op.POST_TYPE  FROM [ORG_EMPLOYEE] oe 
		                 left join ORG_POST  op on oe.POST_ID=op.ID
		                 WHERE oe.STORE_ID=@STORE_ID and oe.STATUS=1 ";
            //and op.POST_TYPE='P0001'
            return base.Query<ORG_EMPLOYEE>(sql, new { STORE_ID = storId });
        }

        //得到空闲技师信息 考虑预约冲突和排版的因素
        public List<ORG_EMPLOYEE> GetFreeEmployee(int storId,int orgid,DateTime beginTime, DateTime endTime, bool? gender)
        {
            string strGender = string.Empty;
            string sql = @"SELECT oe.ID,oe.NAME,op.POST_NAME,op.POST_TYPE 
FROM [ORG_EMPLOYEE] oe LEFT JOIN [ORG_POST] op ON oe.POST_ID =op.ID 
WHERE oe.STORE_ID=@STORE_ID AND oe.STATUS=1 AND op.POST_TYPE='P0001'";
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
WHERE [SCHEDULE_DATE]=convert(date,@BEGIN_Time) AND [ORG_ID]=@ORG_ID AND [STORE_ID]=@STORE_ID) ExpansionT 
WHERE [SCHEDULE_BEGIN_TIME]<=@BEGIN_Time AND [SCHEDULE_END_TIME]>=@END_TIME";

            sql += string.Format(@"
AND oe.ID NOT IN (
{0}) 
AND oe.ID IN (
{1})", conflictSql, scheduleSql);

            return base.Query<ORG_EMPLOYEE>(sql, new { STORE_ID = storId,ORG_ID=orgid,BEGIN_Time = beginTime, END_TIME = endTime, GENDER = strGender });
        }
        

        //得到公司信息
        public List<ORG_INFO> GetAllOrgInfo()
        {
            string sql = "SELECT  [ID],[ORG_NAME]  FROM [ORG_INFO]";
            return base.Query<ORG_INFO>(sql, null);
        }
        
        //得到当前公司门店信息
        public List<ORG_STORE> GetAvailableStoreByOrgId(string orgId)
        {
            string sql = "select [ID],[NAME] from dbo.ORG_STORE where org_Id=@orgId";
            return base.Query<ORG_STORE>(sql, new { orgId = orgId });
        }

        //得到当前公司角色
        public List<SYS_ROLE> GetSYS_ROLEByOrgId(string orgId)
        {
            string sql = "select [ROLE_ID],[ROLE_NAME] from dbo.SYS_ROLE where org_Id=@orgId and ROLE_TYPE<>0";
            return base.Query<SYS_ROLE>(sql, new { orgId = orgId });
        }

     


        //获取产品类别选项
        public List<PROD_CATEGORY> GetProdTypeByOrgId(string orgId, string pNo)
        {
            string sql = @"SELECT * FROM [PROD_CATEGORY] WHERE PARENT_ID='00000000-0000-0000-0000-000000000000' and PROD_TYPE=0  AND ORG_ID=@ORG_ID
               ";
            return base.Query<PROD_CATEGORY>(sql, new { CATE_NO = pNo, ORG_ID = orgId });
        }


        //获取服务项目类别选项 ntt
        public List<PROD_CATEGORY> GetCateTypeByOrgId(string orgId, string ptype)
        {
            string sql = @"select *,0 type,'' fname from PROD_CATEGORY where
PARENT_ID='00000000-0000-0000-0000-000000000000' and PROD_TYPE=@PROD_TYPE  AND ORG_ID=@ORG_ID";
            return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = ptype, ORG_ID = orgId });
        }

        ////获取服务项目类别选项 ntt
        //public List<PROD_CATEGORY> GetScateByPid(string orgId, string ptype,string pid)
        //{
        //    string sql = @"select ID, CATE_NAME from PROD_CATEGORY where PARENT_ID=@PID and ORG_ID=@ORG_ID and PROD_TYPE=@PROD_TYPE";
        //    return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = ptype, ORG_ID = orgId,PID=pid });
        //}

        //获取服务项目类别选项 ntt
        public List<PROD_CATEGORY> GetScateByPid(string orgId, string ptype, string pid)
        {
            if (string.IsNullOrEmpty(pid))
            {
                return new List<PROD_CATEGORY>();
            }
            else
            {

                string sql = @"select * from (select c.*,1 type,b.CATE_NAME fname from PROD_CATEGORY c
left join prod_category b on c.parent_id=b.id where c.PARENT_ID in(select ID from PROD_CATEGORY where PARENT_ID='00000000-0000-0000-0000-000000000000' and PROD_TYPE=@PROD_TYPE AND ORG_ID=@ORG_ID
)) f where PARENT_ID=@PId";
                return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = ptype, ORG_ID = orgId, PId = pid });
            }
        }

        public List<PROD_CATEGORY> GetScateByPname(string orgId, string ptype, string pname)
        {
            if (string.IsNullOrEmpty(pname))
            {
                return new List<PROD_CATEGORY>();
            }
            else
            {
                string sql = @"select * from (select c.*,1 type,b.CATE_NAME fname from PROD_CATEGORY c
left join prod_category b on c.parent_id=b.id where c.PARENT_ID in(select ID from PROD_CATEGORY where PARENT_ID='00000000-0000-0000-0000-000000000000' and PROD_TYPE=@PROD_TYPE AND ORG_ID=@ORG_ID
)) f where fname=@PNAME";
                return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = ptype, ORG_ID = orgId, PNAME = pname });
            }
        }


        /// <summary>
        /// 根据父级DICT_CODE和公司编号获得数据字典
        /// </summary>
        /// <param name="PDICT_CODE">父级DICT_CODE</param>
        /// <param name="ORG_ID">公司编号</param>
        /// <returns></returns>
//        public List<ORG_DICT> GeDICTByPDicCode(string PDICT_CODE, int ORG_ID)
//        {
//            string sql = @"SELECT DICT_CODE,DICT_VALUE FROM [ORG_DICT] DIC WHERE 
//             DIC.PARENT_ID=(SELECT TOP 1 ID FROM ORG_DICT D WHERE D.DICT_CODE=@DICT_CODE AND D.ORG_ID=@ORG_ID) ORDER BY DICT_SEQ";
//            return base.Query<ORG_DICT>(sql, new { DICT_CODE = PDICT_CODE, ORG_ID = ORG_ID });
//        }
        public List<ORG_DICT_EX> GeDICTByPDicCode(string PDICT_CODE, int ORG_ID)
        {
            string sql = @"SELECT DICT_CODE,DICT_VALUE FROM [ORG_DICT] DIC WHERE 
                     DIC.PARENT_ID=(SELECT TOP 1 ID FROM ORG_DICT D WHERE D.DICT_CODE=@DICT_CODE ) ORDER BY DICT_SEQ";
            return base.Query<ORG_DICT_EX>(sql, new { DICT_CODE = PDICT_CODE, ORG_ID = ORG_ID });
        }
        

        /// <summary>
        /// 所属块区
        /// </summary>
        /// <param name="ORG_ID"></param>
        /// <returns></returns>
        public List<ORG_INFO_EX> GeREGION(int ORG_ID)
        {
            string sql = @"WITH PORG_INFO([ID],[ORG_NO],[ORG_NAME],[PARENT_ID],[PROVINCE],[CITY],[REGION]
            ,[ADDRESS],[ZIPCODE],[TELEPHONE],[FAX],[CONTACT],[WEBSITE],[IS_CUST_STORE]
            ,[IS_ONLINE_BOOKING],[SMS_QTY],[EMAIL_QTY],[WECHAT_QTY],[CHANNEL],[SALES]
            ,[STATUS],[REMARK],[ORG_LEVEL]) AS 
            (
                SELECT *
                FROM ORG_INFO
                WHERE ID=@ORG_ID
                UNION ALL
                SELECT A.*
                FROM ORG_INFO A
                    INNER JOIN PORG_INFO B
                    ON A.PARENT_ID = B.ID 
            )
            SELECT C.*
            FROM PORG_INFO C ORDER BY C.ORG_LEVEL";
            return base.Query<ORG_INFO_EX>(sql, new { ORG_ID = ORG_ID });
        }


        /// <summary>
        /// 根据门店编号获取门店列表
        /// </summary>
        /// <param name="storeIds"></param>
        /// <returns></returns>
        public List<ORG_STORE> GeStoresByIds(string storeIds)
        {
            string sql = @"SELECT * FROM ORG_STORE WHERE ID IN (" + storeIds + ")";
            return base.Query<ORG_STORE>(sql, null);
        }


        //产品 项目 大类
        public List<PROD_CATEGORY> getCATEGORY(string prodType)
        {
            string sql = @"SELECT [ID]
                          ,[ORG_ID]
                          ,[CATE_NO]
                          ,[CATE_NAME]
                          ,[PARENT_ID]
                          ,[CATE_SEQ]
                          ,[PROD_TYPE]
                      FROM [PROD_CATEGORY] WHERE PROD_TYPE=@PROD_TYPE";
            return base.Query<PROD_CATEGORY>(sql, new { PROD_TYPE = prodType });
        }

        

        public List<ORG_DICT> GeDICTByPDicCode(string PDICT_CODE)
        {
            string sql = @"SELECT distinct DICT_CODE,DICT_VALUE FROM [ORG_DICT] DIC WHERE 
                     DIC.PARENT_ID=(SELECT TOP 1 ID FROM ORG_DICT D WHERE D.DICT_CODE=@DICT_CODE ) ";
            return base.Query<ORG_DICT>(sql, new { DICT_CODE = PDICT_CODE});
        }

        /// <summary>
        /// 获取所有图文列表
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<WXGraphicList> GetGraphicList(string Merchants_ID)
        {
            string sql = "select * from wxGraphicList where Merchants_ID='" + Merchants_ID + "'";
            return base.Query<WXGraphicList>(sql, null);
        }

        /// <summary>
        /// 获取门店
        /// </summary>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public List<ORG_STORE> GetORG_STORE(string Merchants_ID)
        {
            string sql = "select * from ORG_STORE where ORG_ID='" + Merchants_ID + "'";
            return base.Query<ORG_STORE>(sql, null);
        }
    }
}
