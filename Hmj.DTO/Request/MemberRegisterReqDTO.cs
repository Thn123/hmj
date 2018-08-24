using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class MemberRegisterReqDTO
    {
        /// <summary>
        /// 生日
        /// </summary>
        public string BIRTHDAY { get; set; }

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
        /// 手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// OPENID
        /// </summary>
        public string OPENID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NICK_NAME { get; set; }

        /// <summary>
        /// 推荐人手机号
        /// </summary>
        public string REFEREE_MOBILE { get; set; }
    }
}
