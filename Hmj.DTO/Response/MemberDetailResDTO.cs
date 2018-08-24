using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class MemberDetailResDTO
    {

        /// <summary>
        /// 剩余积分
        /// </summary>
        public int? AVA_POINTS { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        public string MEM_LEVEL { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NICK_NAME { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        public string IMAGE { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string NAME_LAST { get; set; }

        /// <summary>
        ///  名
        /// </summary>
        public string NAME_FIRST { get; set; }

        /// <summary>
        ///  性别 0：女 1：男
        /// </summary>
        public string GENDER { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public string BIRTHDAY { get; set; }

        /// <summary>
        /// BP号
        /// </summary>
        public string PARTNER { get; set; }

        /// <summary>
        /// 是否已经完善信息并发送积分
        /// </summary>
        public string IS_SEND { get; set; }

        /// <summary>
        /// 兑换密码
        /// </summary>
        public string LOGINPASSON { get; set; }

        /// <summary>
        /// 会员状态
        /// </summary>
        public string ZZAFLD000004 { get; set; }

        /// <summary>
        /// 会员编号
        /// </summary>
        public string MEMBERNO { get; set; }
    }
}
