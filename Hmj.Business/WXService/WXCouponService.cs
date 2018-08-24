using Hmj.DataAccess.WXRespository;
using Hmj.Interface.WXService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Business.WXService
{
    public class WXCouponService : IWXCouponService
    {
        private WXCouponRepository _set;
        public WXCouponService()
        {
            _set = new WXCouponRepository();
        }
        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId)
        {
            return _set.UpdateWXCouponIsGet(OpenId, CouponNo, CardId);
        }


        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId)
        {
            return _set.UpdateWXCouponIsHX(OpenId, CouponNo, CardId);
        }

    }
}
