using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Interface.WXService
{
    public interface IWXCouponService
    {


        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId);

        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId);

    }
}
