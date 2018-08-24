using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class BindReqDTO
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string MOBILE { get; set; }

        /// <summary>
        /// 微信粉丝唯一标示
        /// </summary>
        public string OPENID { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NICKNAME { get; set; }

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
        /// 会员当前状态（是激活，注销还是别的什么）
        /// </summary>
        public string ZZAFLD000004 { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public string REG_DATE { get; set; }
    }
}
