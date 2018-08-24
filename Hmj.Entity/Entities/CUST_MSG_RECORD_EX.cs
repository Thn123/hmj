using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class CUST_MSG_RECORD_EX:WXCUST_MSG_RECORD
    {
        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string IMAGE { get; set; }
        /// <summary>
        /// 微信好友昵称
        /// </summary>
        public string Fname { get; set; }
        /// <summary>
        /// 图文
        /// </summary>
        public string Tuwen { get; set; }
        /// <summary>
        /// 粉丝ID
        /// </summary>
        public int FID { get; set; }
        /// <summary>
        /// 表情
        /// </summary>
        public string Biaoqing { get; set; }

        public List<WXBiaoqing> BQlist { get; set; }
    }
}
