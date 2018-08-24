namespace Hmj.Entity.Entities
{
    public class CUST_FANS_EX:WXCUST_FANS
    {
        public string qx { get; set; }

        public string xb { get; set; }

        public string Message { get; set; }

        public int EcId { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public string MEM_LEVEL { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string XM { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        public string Yzm { get; set; }
        public string HTML { get; set; }
    }
}
