using System;

namespace Hmj.Entity.WxModel
{
    /// <summary>
    /// 会员的详情
    /// </summary>
    public class MemberInfo
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Headimgurl { get; set; }

        /// <summary>
        /// OpenID
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 会员可用积分
        /// </summary>
        public int? AVA_POINTS { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// 性別 0：女 1：男
        /// </summary>
        public int SEX { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string CITY_NAME { get; set; }

        /// <summary>
        /// 省份名称
        /// </summary>
        public string PROVINCE_NAME { get; set; }





        /// <summary>
        /// 全名
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 业务伙伴（个人）的姓
        /// </summary>
        public string NAME_LAST { get; set; }

        /// <summary>
        /// 业务伙伴（个人）的名字 
        /// </summary>
        public string NAME_FIRST { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string ADDRESS { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public string MEM_LEVEL { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? BIRTHDAY { get; set; }

        /// <summary>
        /// 门店
        /// </summary>
        public string STORE { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string STORE_NAME { get; set; }

        /// <summary>
        /// 兑换密码
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// 优惠券数量
        /// </summary>
        public string COUPON_COUNT { get; set; }

        /// <summary>
        /// 会员ID
        /// </summary>
        public string Member_Id { get; set; }
    }
}
