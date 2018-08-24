namespace Hmj.Entity.Entities
{
    public  class HMJ_BOOK_EX : HMJ_BOOK
    {
        /// <summary>
        /// 预约会员名称
        /// </summary>
        public string MEMBER_NAME { get; set; }

        /// <summary>
        /// 预约会员手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// 门店名称
        /// </summary>
        public string STORE_NAME { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string BOOK_DATE_EX { get; set; }
    }
}
