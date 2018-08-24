using System;

namespace Hmj.Entity.Entities
{
    public class PointHisList
    {
        /// <summary>
        /// 积分变动日期
        /// </summary>
        public string CREATED_TIME { get; set; }

        /// <summary>
        /// 积分变动日期，扩展
        /// </summary>
        public DateTime CREATED_TIME_EX { get; set; }

        /// <summary>
        /// 积分有效期
        /// </summary>
        public string EXPIRE_DATE { get; set; }

        /// <summary>
        /// 积分值
        /// </summary>
        public string POINTS { get; set; }

        /// <summary>
        /// 积分方向 0：增加 1：减少
        /// </summary>
        public string CATEGORY { get; set; }

        /// <summary>
        /// 积分变动类型
        /// </summary>
        public string ORDER_TYPE { get; set; }
        
    }
}
