using Hmj.DataAccess.Repository;
using Hmj.Entity;

namespace Hmj.Business.WXService
{
    public class WXPersonInfoService
    {
        private WXPersonInfoRepository _WXPersonInfoRepository;
        private WXPersonInfoRepository WXPersonInfoRepository
        {
            get
            {
                if (_WXPersonInfoRepository == null)
                {
                    _WXPersonInfoRepository = new WXPersonInfoRepository();
                }
                return _WXPersonInfoRepository;
            }
        }

        /// <summary>
        /// 保存微信会员对象
        /// </summary>
        /// <param name="pi">WXPersonInfo对象</param>
        /// <returns>整型</returns>
        public int Save(WXPersonInfo pi)
        {
            if (pi.ID <= 0)
            {
                return (int)WXPersonInfoRepository.Insert(pi);
            }
            else
            {
                return WXPersonInfoRepository.Update(pi);
            }
        }

        /// <summary>
        ///  查询微信会员对象
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>WXPersonInfo对象</returns>
        public WXPersonInfo Get(int id)
        {
            return WXPersonInfoRepository.Get(id);
        }

        /// <summary>
        /// 通过fromUserName查询微信会员对象
        /// </summary>
        /// <param name="fromUserName"></param>
        /// <param name="orgID">商户ID</param>
        /// <returns>WXPersonInfo对象</returns>
        public WXPersonInfo GetByFromUserName(string fromUserName, int orgID)
        {
            return WXPersonInfoRepository.GetByFromUserName(fromUserName, orgID);
        }

        /// <summary>
        /// 通过phone查询微信会员对象
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="orgID">商户ID</param>
        /// <returns>WXPersonInfo对象</returns>
        public WXPersonInfo GetByPhone(string phone, int orgID)
        {
            return WXPersonInfoRepository.GetByPhone(phone, orgID);
        }

    }
}
