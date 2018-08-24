using Hmj.Entity;

namespace Hmj.DataAccess.Repository
{
    public class OpinionRepository : BaseRepository
    {
        public int AddOpinion(WXOpinion op)
        {
            return (int)base.Insert(op);
        }

        /// <summary>
        /// /根据openid或手机号码获取会员信息
        /// </summary>
        /// <param name="fromusername"></param>
        /// <returns></returns>
        public CUST_INFO GetCustinfoByFromusername(string fromusername)
        {
            string sql = "SELECT TOP 1 * FROM dbo.CUST_INFO WHERE FROM_USER_NAME=@user OR MOBILE=@phone";
            return Get<CUST_INFO>(sql, new { user = fromusername, phone = fromusername });
        }

        /// <summary>
        /// 修改会员绑定
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        public int UpdateCust(CUST_INFO cust)
        {
            if (cust.ID == 0)
                return (int)Insert(cust);
            else
            return Update(cust);
        }

    }
}
