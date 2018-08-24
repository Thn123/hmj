using Hmj.Entity;
using Hmj.Entity.Entities;
using Hmj.Entity.PageSearch;
using System.Collections.Generic;

namespace Hmj.DataAccess.Repository
{
    public class HmjCommonRepository : BaseRepository
    {
        /// <summary>
        /// 得到一个实体类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<HMJ_CITY> GetAdministrativeDivision()
        {
            string sql = "SELECT * FROM  dbo.HMJ_CITY";
            return base.Query<HMJ_CITY>(sql, new { });
        }

        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public List<BCJ_CITY> GetCity()
        {
            string sql = "select ID,CITY_CODE,CITY_NAME,LATITUDE,LONGITUDE from BCJ_CITY";

            return base.Query<BCJ_CITY>(sql, new { });
        }

        /// <summary>
        /// 得到门店数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<BCJ_STORES_EX> GetStoresByCityCode(string code)
        {
            string sql = @"SELECT 
                            ID,
                            POS_CODE,
                            STORE_CODE,
                            STORE_NAME,
                            STORE_REG,
                            PROVINCE,
                            CITY,
                            CITY_CATE,
                            PATTERN,
                            ELITE,
                            STORE_TYPE,
                            NAME,
                            ADDRESS,
                            TEL,
                            SYSTEM,
                            IS_UPLOAD,
                            KUNNR_SH,
                            LATITUDE,LONGITUDE,
                            STATUS,
                            IS_YM_STORE,
                            PASSWORD,
                            vgroup,
                            source
                            FROM dbo.BCJ_STORES 
                            WHERE STATUS=0 
                            AND STORE_TYPE NOT IN('03','04') 
                            AND isnull(LATITUDE,'无') !='无' ";

            return base.Query<BCJ_STORES_EX>(sql, new { CITY = code });
        }
    }
}
