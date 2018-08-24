using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class MEMBER_COUPON
    {
        /// <summary>
        /// 已使用
        /// </summary>
        public List<COUPONS> ALREADY_USE { get; set; }

        /// <summary>
        /// 未使用
        /// </summary>
        public List<COUPONS> NOT_USE { get; set; }

        /// <summary>
        /// 已作废
        /// </summary>
        public List<COUPONS> OBSOLETE { get; set; }
    }
}
