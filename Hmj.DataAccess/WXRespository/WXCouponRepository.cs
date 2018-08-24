using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DataAccess.WXRespository
{
    public class WXCouponRepository : CouponBaseRepository
    {

        /// <summary>
        /// 标志卡券已经领取
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsGet(string OpenId, string CouponNo, string CardId)
        {
            string sql = "update WXCouponGiveInfo set Status=1 ,CouponGetDate=getdate(),CouponNo=@CouponNo where  CardId=@CardId and OpenId=@OpenId and isnull(Status,0)=0";
            return base.Excute(sql, new
            {
                CouponNo = CouponNo,
                CardId = CardId,
                OpenId = OpenId
            });
        }

        /// <summary>
        /// 标志卡券已经核销
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public int UpdateWXCouponIsHX(string OpenId, string CouponNo, string CardId)
        {
            string sql = "update WXCouponGiveInfo set Status=2 ,UseDate=getdate() where  CardId=@CardId and OpenId=@OpenId and CouponNo=@CouponNo and isnull(Status,0)=1";
            return base.Excute(sql, new
            {
                CouponNo = CouponNo,
                CardId = CardId,
                OpenId = OpenId
            });
        }
    }
}
