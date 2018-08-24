using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class CrmMemberResDTO
    {
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
        /// 会员状态
        /// </summary>
        public string ZZAFLD000004 { get; set; }

        /// <summary>
        /// 会员注册日期
        /// </summary>
        public string REG_DATE { get; set; }

        public string ACCOUNT_ID { get; set; }


    }
}
