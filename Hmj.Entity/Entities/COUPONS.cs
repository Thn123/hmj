using System;

namespace Hmj.Entity.Entities
{
    public class COUPONS
    {
        
        /// <summary>
        /// 结束时间扩展
        /// </summary>
        public DateTime ZCP_EDATE_EX { get; set; }

        /// <summary>
        /// 优惠券号
        /// </summary>
        public string ZCP_YHQ { get; set; }

        /// <summary>
        /// 会员账号
        /// </summary>
        public string BPEXT { get; set; }
        
        /// <summary>
        /// 优惠券类型
        /// </summary>
        public string ZCP_TYPE { get; set; }
        
        /// <summary>
        /// 优惠券描述
        /// </summary>
        public string ZCP_YHQDES { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime ZCP_EDATE { get; set; }

        /// <summary>
        /// 面值
        /// </summary>
        public string ZCP_JE { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string ZCP_ZK { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public string ZCP_POINT { get; set; }
        
        /// <summary>
        /// 物料
        /// </summary>
        public string ZCP_PROD { get; set; }

        /// <summary>
        /// 优惠券券码
        /// </summary>
        public string ZCP_NUM { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime ZCP_BDATE { get; set; }

        /// <summary>
        /// 校验密码
        /// </summary>
        public string ZCP_PASSW { get; set; }

        /// <summary>
        /// 使用日期
        /// </summary>
        public DateTime? ZCPUDATE { get; set; }

        /// <summary>
        /// 是否预约 0：未预约 1：已经预约
        /// </summary>
        public int IS_BOOK { get; set; }

        //二维码
        public string QrCode { get; set; }

        public string CONTENT { get; set; }

        public string RULE { get; set; }

        public string ZCP_USE_FLAG { get; set; }

    }
}
