using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Entity.Entities
{
    public class HmjMemberDetail:CUST_MEMBER
    {
        /// <summary>
        /// 昵称
        /// </summary>
        public string NICK_NAME { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string IMAGE { get; set; }

        /// <summary>
        /// 是否已经完善信息并发送积分
        /// </summary>
        public string IS_SEND { get; set; }
    }
}
