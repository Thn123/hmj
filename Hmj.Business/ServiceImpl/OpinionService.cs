using Hmj.DataAccess.Repository;
using Hmj.Entity;

namespace Hmj.Business.ServiceImpl
{
    public class OpinionService
    {
        OpinionRepository _op = new OpinionRepository();

        /// <summary>
        /// /根据openid或手机号码获取会员信息
        /// </summary>
        /// <param name="fromusername"></param>
        /// <returns></returns>
        public CUST_INFO GetCustinfoByFromusername(string fromusername)
        {
            return _op.GetCustinfoByFromusername(fromusername);
        }

        /// <summary>
        /// 修改会员绑定
        /// </summary>
        /// <param name="cust"></param>
        /// <returns></returns>
        public int UpdateCust(CUST_INFO cust)
        {
            return _op.UpdateCust(cust);
        }
    }
}
