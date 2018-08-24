using Hmj.Common;
using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class StoreReponsitory : BaseRepository
    {
        public PagedList<ORG_STORE_EX> QueryAllStoreDate(StoreSearch search, PageView view)
        {
            string sql = @"A.[ID],A.[STORE_NO],A.[ORG_ID],B.[ORG_NAME],A.[NAME],A.[TYPE],A.[BRAND],A.[PROVINCE],A.[CITY],A.[REGION]
             ,A.[ADDRESS],A.[MANAGER],A.[TELEPHONE],A.[OPEN_DATE],A.[CLOSE_DATE],A.[STATUS],A.[SERVICE_TYPE]";
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(search.SNAME))
            {
                sqlWhere += " AND A.[NAME] LIKE '%" + search.SNAME + "%'";
            }
            if (!string.IsNullOrEmpty(search.STYPE))
            {
                sqlWhere += " AND A.[TYPE] = '" + search.STYPE + "'";
            }
            if (!string.IsNullOrEmpty(search.SSTATUS))
            {
                sqlWhere += " AND A.[STATUS] = '" + search.SSTATUS + "'";
            }

            if (search.REGION_ID != -1 && search.ORG_ID!=search.REGION_ID)
            {
                string str_id = "";
                 GetChildID(search.REGION_ID, ref str_id);
                 str_id += search.REGION_ID;
                 sqlWhere += " AND A.[REGION_ID] in (" + str_id + ")";
            }

            sqlWhere += " AND A.[ORG_ID] = " + search.ORG_ID;

            return base.PageGet<ORG_STORE_EX>(view, sql, "[ORG_STORE] A left join ORG_INFO B on A.REGION_ID=B.ID ",
                   sqlWhere, "A.[ID] Desc", "");
        } 
          
        public ORG_STORE_EX QueryStoreById(int id)
        {
            string s = AppConfig.ImageUrl;
            string sql = @"SELECT A.*, Replace(Replace(c.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') IMAGE_ID_URL,Replace(Replace(d.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') MIMAGE_ID_URL,Replace(Replace(e.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') SIMAGE_ID_URL,
convert(varchar,A.OPEN_DATE,23) OPEN_DATE_Str,convert(varchar,A.CLOSE_DATE,23) CLOSE_DATE_Str,B.ORG_NAME,F.ORG_NAME REGION_NAME
FROM [ORG_STORE] A left join ORG_INFO B on A.ORG_ID=B.ID  
left join Files C on a.image_id=c.id 
left join Files D on a.mimage_id=d.id 
left join Files E on a.simage_id=e.id 
left join ORG_INFO F on A.REGION_ID=F.ID
 where A.id=@xId and a.status=1";
            return base.Get<ORG_STORE_EX>(sql, new { xId = id });
        }

        public ORG_STORE_EX QueryStoreById_Ex(int id)
        {
            string sql = @"SELECT A.* FROM [ORG_STORE] A where A.id=@xId and a.status=1";
            return base.Get<ORG_STORE_EX>(sql, new { xId = id });
        }

        public int SaveValidateStoreById(ORG_STORE cuStore)
        {
            string sql = @"SELECT [ID] FROM [ORG_STORE] where STORE_NO=@STORE_NO and id<>@id AND ORG_ID=@ORG_ID ";
            return base.Get<int>(sql, new { STORE_NO = cuStore.STORE_NO, id = cuStore.ID, ORG_ID = cuStore.ORG_ID });
        }

        public int DeleteStore(int id)
        {
            string sql = "DELETE FROM [ORG_STORE] WHERE [ID]=@ID";
            return base.Excute(sql, new { ID = id });
        }

        public int DeteStoreItemByType(int id, int type)
        {
            string sql = "DELETE FROM [REL_STORE_PROD] WHERE PROD_ID=@ProdID AND [PROD_TYPE]=@Type";
            return base.Excute(sql, new { ProdID = id, Type = type });
        }

        public ORG_STORE QueryBaseStoreById(int Id)
        {
            string sql = @"SELECT A.* From [ORG_STORE] A  WHERE A.ID=@ID";
            return base.Get<ORG_STORE>(sql, new { ID = Id });
        }
        public List<ORG_STORE_EX> QueryStoreByOrgId(int orgId)
        {
            string s = AppConfig.ImageUrl;
            string sql = @"SELECT A.*, Replace(Replace(c.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') IMAGE_ID_URL,Replace(Replace(d.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') MIMAGE_ID_URL,Replace(Replace(e.file_url,'/helper','" + s + @"'),'/customer','" + s + @"') SIMAGE_ID_URL,
convert(varchar,A.OPEN_DATE,23) OPEN_DATE_Str,convert(varchar,A.CLOSE_DATE,23) CLOSE_DATE_Str
FROM [ORG_STORE] A  
left join Files C on a.image_id=c.id 
left join Files D on a.mimage_id=d.id 
left join Files E on a.simage_id=e.id 
where a.ORG_ID=@ORG_ID and a.status=1
";
            return base.Query<ORG_STORE_EX>(sql, new { ORG_ID = orgId });
        }

        public List<ORG_STORE_EX> QueryStoreByOrgId_Ex(int orgId)
        {
            string sql = @"SELECT A.*
                        FROM [ORG_STORE] A  
                        where a.ORG_ID=@ORG_ID and a.status=1
                        ";
            return base.Query<ORG_STORE_EX>(sql, new { ORG_ID = orgId });
        }

        public List<ORG_STORE_EX> QueryStoreByOrgIdAndReg(int orgId, int REGION_ID)
        {
            string sql = @"SELECT A.* From [ORG_STORE] A  WHERE A.ORG_ID=@ORG_ID AND A.REGION_ID=@REGION_ID";
            return base.Query<ORG_STORE_EX>(sql, new { ORG_ID = orgId, REGION_ID = REGION_ID });
        }
        public List<ORG_STORE_EX> QueryStoreData(int orgId, string userType, int REGION_ID)
        {
            string sql = @"SELECT A.* From [ORG_STORE] A  WHERE A.ORG_ID=@ORG_ID";

            //区域用户
            if (userType == "1")
            {
                sql = @"WITH PORG_INFO([ID],[ORG_NO],[ORG_NAME],[PARENT_ID],[PROVINCE],[CITY],[REGION]
            ,[ADDRESS],[ZIPCODE],[TELEPHONE],[FAX],[CONTACT],[WEBSITE],[IS_CUST_STORE]
            ,[IS_ONLINE_BOOKING],[SMS_QTY],[EMAIL_QTY],[WECHAT_QTY],[CHANNEL],[SALES]
            ,[STATUS],[REMARK],[ORG_LEVEL]) AS 
            (
                SELECT *
                FROM ORG_INFO
                WHERE ID=" + REGION_ID + @"
                UNION ALL
                SELECT A.*
                FROM ORG_INFO A
                    INNER JOIN PORG_INFO B
                    ON A.PARENT_ID = B.ID 
            )
             SELECT A.* From [ORG_STORE] A 
             WHERE A.ORG_ID=@ORG_ID AND A.REGION_ID in 
            (SELECT convert(varchar(20),C.ID)
            FROM PORG_INFO C) ";
            }
            //门店用户
            else if (userType == "2")
            {
                sql += @" AND A.ID=" + REGION_ID;
            }
            return base.Query<ORG_STORE_EX>(sql, new { ORG_ID = orgId });
        }

        public PagedList<ORG_STORE_EX> QueryAllStoreDateEx(StoreSearch search, PageView view)
        {
            string sql = @"A.[ID],A.[STORE_NO],A.[ORG_ID],B.[ORG_NAME],A.[NAME],A.[TYPE],A.[BRAND],A.[PROVINCE],A.[CITY],A.[REGION]
             ,A.[ADDRESS],A.[MANAGER],A.[TELEPHONE],A.[OPEN_DATE],A.[CLOSE_DATE],A.[STATUS]";
            string sqlWhere = "";
            if (!string.IsNullOrEmpty(search.SNAME))
            {
                sqlWhere += " AND A.[NAME] LIKE '%" + search.SNAME + "%'";
            }
            if (!string.IsNullOrEmpty(search.STYPE))
            {
                sqlWhere += " AND A.[TYPE] = '" + search.STYPE + "'";
            }
            if (!string.IsNullOrEmpty(search.SSTATUS))
            {
                sqlWhere += " AND A.[STATUS] = '" + search.SSTATUS + "'";
            }

            //if (search.REGION_ID != -1 && search.ORG_ID != search.REGION_ID)
            //{
            //    sqlWhere += " AND A.[REGION_ID] = " + search.REGION_ID;
            //}


            return base.PageGet<ORG_STORE_EX>(view, sql, "[ORG_STORE] A left join ORG_INFO B on A.REGION_ID=B.ID ",
                   sqlWhere, "A.[ID] Desc", "");
        }


        public void GetChildID(int PARENT_ID, ref string  str_id)
        {
            string sql = "select * from ORG_INFO";
            List<ORG_INFO_EX> cuList = base.Query<ORG_INFO_EX>(sql,null);
            GetChildsID(cuList, PARENT_ID, ref str_id);
        }

        public void GetChildsID(List<ORG_INFO_EX> cuList,int PARENT_ID, ref string str_id)
        {
            //var childList = cuList.Where(c => c.PARENT_ID == PARENT_ID);
            //foreach (var p in childList)
            //{
            //    str_id += p.ID + ",";
            //    GetChildsID(cuList, p.ID, ref str_id);
            //}
        }

        public List<ORG_INFO_EX> GetORG_INFO_BY_ID(int ORG_ID)
        {
            string str_id = "";
            GetChildID(ORG_ID, ref str_id);
            str_id += ORG_ID;
            string sql = "select * from ORG_INFO WHERE ID IN (" + str_id + ")";
            return base.Query<ORG_INFO_EX>(sql, null);
        }

        public WXStore GetStore(int id)
        {
            return base.Get<WXStore>(@"SELECT TOP 1 * FROM [WXStore] WHERE ID=@ID", new { ID = id });
        }

        public List<WXStore> GetStores(int merchants_ID)
        {
            return base.Query<WXStore>(@"SELECT * FROM [WXStore] WHERE Merchants_ID=@Merchants_ID", new { Merchants_ID = merchants_ID });
        }

        /// <summary>
        /// 根据条件查询门店
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public List<ORG_STORE> GetStoreList(string strwhere)
        {
            return base.Query<ORG_STORE>("select * from ORG_STORE where "+strwhere,null);
        }

        /// <summary>
        /// 根据条件查询门店
        /// </summary>
        /// <param name="strwhere"></param>
        /// <returns></returns>
        public List<ORG_STORE> GetStoreListBySql(string sql)
        {
            return base.Query<ORG_STORE>(sql, null);
        }


    }
}
