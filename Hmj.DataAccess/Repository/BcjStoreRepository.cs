using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using Hmj.Entity.SearchEntity;
using System;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class BcjStoreRepository : BaseRepository
    {
        /// <summary>
        /// 根据门店的编号得到门店的信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public BCJ_STORES GetStoreByCode(string storeCode, string pwd = "")
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(pwd))
            {
                str = " AND PASSWORD='" + pwd + "'";
            }

            string sql = "SELECT * FROM dbo.BCJ_STORES WHERE POS_CODE=@POS_CODE AND IS_YM_STORE=1 " + str;

            return base.Get<BCJ_STORES>(sql, new { POS_CODE = storeCode });
        }

        /// <summary>
        /// 得到门店数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<BCJ_STORES_EX> GetStoresByCityCode(string code)
        {
            string sql = @"SELECT * FROM dbo.BCJ_STORES WHERE CITY=@CITY AND STATUS=0 AND 
STORE_TYPE NOT IN('03','04') AND LATITUDE !='无' AND IS_YM_STORE=1 and POS_CODE not like 'J%'";//J开头门店编码为办公

            return base.Query<BCJ_STORES_EX>(sql, new { CITY = code });
        }


        public List<MEMBER_EX> GetMembers()
        {
            string sql = @"SELECT A.ID,A.MEMBERNO,B.FROMUSERNAME OPENID FROM dbo.CUST_MEMBER A
LEFT JOIN dbo.WXCUST_FANS B ON A.FANS_ID = B.ID";


            List<MEMBER_EX> LIST = base.Query<MEMBER_EX>(sql, new { });

            string ids = string.Empty;

            if (LIST != null && LIST.Count > 0)
            {
                foreach (MEMBER_EX item in LIST)
                {
                    ids += item.ID.ToString() + ",";
                }

                string sql2 = @"UPDATE CUST_MEMBER SET IS_SEND_TMP=1 WHERE ID IN(" + ids.Trim(',') + ")";

                base.Excute(sql2, new { });
            }

            return LIST;
        }

        /// <summary>
        /// 得到门店详细信息
        /// </summary>
        /// <param name="storeCode"></param>
        /// <returns></returns>
        public BCJ_STORES_EX GetStoreEntity(string storeCode)
        {
            string sql = @"SELECT B.CITY_NAME,A.* FROM BCJ_STORES A
LEFT JOIN dbo.BCJ_CITY B ON A.CITY = B.CITY_CODE
WHERE A.POS_CODE = @POS_CODE";

            return base.Get<BCJ_STORES_EX>(sql, new { POS_CODE = storeCode });
        }

        /// <summary>
        /// 得到配置列表
        /// </summary>
        /// <param name="template_Code"></param>
        /// <returns></returns>
        public List<WX_TMP_CONFIG> GetTmps(string template_Code)
        {
            string sql = "SELECT * FROM WX_TMP_CONFIG WHERE TMP_ID = @TMP_ID";

            return base.Query<WX_TMP_CONFIG>(sql, new { TMP_ID = template_Code });
        }

        /// <summary>
        /// 分两步，第一步得到列表第二步更新信息
        /// </summary>
        /// <returns></returns>
        public List<WX_TMP_HIS> GetTmpHis()
        {
            DateTime data = DateTime.Now;
            string sql = @"SELECT ID,DETAIL,OPENID,TMP_ID FROM dbo.WX_TMP_HIS WHERE IS_SELECT=0 AND IS_SEND=0 AND SEND_TIME BETWEEN @BEGIN AND @END;
UPDATE WX_TMP_HIS SET IS_SELECT = 1 WHERE ID IN(SELECT ID FROM dbo.WX_TMP_HIS WHERE IS_SELECT=0 AND IS_SEND=0 AND SEND_TIME BETWEEN @BEGIN AND @END);";
            return base.Query<WX_TMP_HIS>(sql, new { BEGIN = data.AddMinutes(-3), END = data });
        }

        /// <summary>
        /// 标记发送
        /// </summary>
        /// <param name="iD"></param>
        public void UpdateOk(int iD)
        {
            string sql = "UPDATE WX_TMP_HIS SET IS_SEND=1 WHERE ID=@ID";

            base.Excute(sql, new { ID = iD });
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        public PagedList<STORES_EXCEL> QueryExcels(ExcelSearch search, PageView view)
        {
            string wherestr = "AND B.NAME IS NOT NULL GROUP BY B.ID,B.NAME,B.POS_CODE";

            return base.PageGet<STORES_EXCEL>(view, @" B.ID STORE_ID,replace(B.NAME,' ','') STORE_NAME,B.POS_CODE STORE_CODE,
COUNT(0) FANS_COUNT, SUM(CASE WHEN C.ID IS NULL THEN 0 ELSE 1 END) MEMBER_COUNT ", @" dbo.WXCUST_FANS A
LEFT JOIN dbo.BCJ_STORES B ON A.STORE_CODE = B.POS_CODE
LEFT JOIN CUST_MEMBER C ON A.ID = C.FANS_ID ", wherestr, " B.ID ", "order by B.ID");
        }

        public List<BCJ_STORES> QueryBcjStores()
        {
            string sql = @"select * from BCJ_STORES where isnull(LATITUDE,'')='' and isnull(ADDRESS,'')!=''";
            return base.Query<BCJ_STORES>(sql, new { });
        }
    }
}
