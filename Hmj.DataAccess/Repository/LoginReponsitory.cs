using Hmj.Entity;
using Hmj.Entity.Entities;

namespace Hmj.DataAccess.Repository
{
    public class LoginReponsitory : BaseRepository
    {
        public USER_INFO_EX GetUserById(int id)
        {
            string sql = @"SELECT A.*,B.NAME EMPLOYEE_NAME,B.ORG_ID,D.ORG_NAME,
(CASE WHEN A.USER_TYPE='0' OR A.USER_TYPE='1' THEN 
(SELECT O.ORG_NAME FROM ORG_INFO O WHERE O.ID=A.STORE_ID)
WHEN A.USER_TYPE='2' THEN 
(SELECT O.NAME FROM ORG_STORE O WHERE O.ID=A.STORE_ID)
ELSE '' END) STORE_NAME FROM [USER_INFO] A 
                 LEFT JOIN ORG_EMPLOYEE B ON A.EMPLOYEE_ID=B.ID  
                 LEFT JOIN ORG_INFO D ON B.ORG_ID=D.ID  
                where A.ID=@id";
            var cuo = base.Get<USER_INFO_EX>(sql, new { id = id });
            return cuo;
        }


        public USER_INFO_EX LoginSYSUser(string loginName, string pwd)
        {
            string sql = @"select * from SYS_USER_INFO WHERE USER_NO=@USER_NO AND USER_PASS=@USER_PASS";
            var cuo = base.Get<USER_INFO_EX>(sql, new { USER_NO = loginName, USER_PASS = pwd });
            return cuo;
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(int id)
        {
            return base.Get<ORG_INFO>("select * from ORG_INFO where id=" + id, null);
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchantsBy(string TOUSERNAME)
        {
            return base.Get<ORG_INFO>("SELECT * FROM dbo.ORG_INFO WHERE TOUSERNAME='" + TOUSERNAME + "'", null);
        }

        /// <summary>
        /// 修改商户信息
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public int UpdateMerchants(ORG_INFO m)
        {
            return Update(m);
        }

        public USER_INFO_EX WXLoginUser(string loginName, string pwd, string ORG_NO)
        {
            string sql = @"SELECT A.*,m.ORG_NAME  STORE_NAME  FROM [USER_INFO] A left join ORG_INFO m on a.ORG_ID=m.ID  
 where USER_NO=@USER_NO and USER_PASS=@USER_PASS and m.ID=@ORG_NO";
            var cuo = base.Get<USER_INFO_EX>(sql, new { USER_NO = loginName, USER_PASS = pwd, ORG_NO = ORG_NO });
            return cuo;
        }

        public USER_INFO_EX WXGetUserById(int id)
        {
            //            string sql = @"SELECT A.*,B.NAME EMPLOYEE_NAME,B.ORG_ID,D.ORG_NAME,
            //(CASE WHEN A.USER_TYPE='0' OR A.USER_TYPE='1' THEN 
            //(SELECT O.ORG_NAME FROM ORG_INFO O WHERE O.ID=A.STORE_ID)
            //WHEN A.USER_TYPE='2' THEN 
            //(SELECT O.NAME FROM ORG_STORE O WHERE O.ID=A.STORE_ID)
            //ELSE '' END) STORE_NAME FROM [USER_INFO] A 
            //                 LEFT JOIN ORG_EMPLOYEE B ON A.EMPLOYEE_ID=B.ID 
            //                 LEFT JOIN ORG_INFO D ON B.ORG_ID=D.ID  
            //                where A.ID=@id";
            string sql = @"SELECT A.*,m.ORG_NAME  STORE_NAME  FROM [USER_INFO] A left join ORG_INFO m on a.ORG_ID=m.ID where A.ID=@ID";
            var cuo = base.Get<USER_INFO_EX>(sql, new { ID = id });
            return cuo;
        }

        /// <summary>
        /// 获取商户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ORG_INFO GetMerchants(string ToUserName)
        {
            return base.Get<ORG_INFO>(@" SELECT * FROM dbo.ORG_INFO where ToUserName=@ToUserName", new { ToUserName = ToUserName });
        }
    }
}
