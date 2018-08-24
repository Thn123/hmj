namespace Hmj.Entity.Entities
{
    /// <summary>
    /// 首页仪表盘
    /// </summary>
    public class Index_Report_EX
    {
        /// <summary>
        /// 营业收入
        /// </summary>
        public string YYSR { get; set; }

        /// <summary>
        /// 会员卡收入
        /// </summary>
        public string HYKSR { get; set; }

        /// <summary>
        /// 新增会员
        /// </summary>
        public string XZHY { get; set; }

        /// <summary>
        /// 新增预约
        /// </summary>
        public string XZYY { get; set; }

        public string[] ArrGoodsSalesAnalays { get; set; }//产品销售分析

        public string[] ArrCardSalesAnalays { get; set; }//卡销售分析

        public string[] ArrProSalesAnalays { get; set; }//项目销售分析

        public string[] ArrMem { get; set; }//会员入会数

        public string[] ArrTraveler { get; set; }//散客消费数

        public string[] ArrTotalSum { get; set; }//总消费数

        /// <summary>
        /// 未读消息
        /// </summary>
        public string C1 { get; set; }
        /// <summary>
        /// 新增粉丝
        /// </summary>
        public string C2 { get; set; }
        /// <summary>
        /// 跑路粉丝
        /// </summary>
        public string C3 { get; set; }
        /// <summary>
        /// 现有粉丝
        /// </summary>
        public string C4 { get; set; }
    }
}
