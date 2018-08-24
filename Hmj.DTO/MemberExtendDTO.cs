using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class MemberExtendDTO
    {
        /// <summary>
        /// 大宝生日
        /// </summary>
        public string KID_BIRTHDAY { get; set; }

        /// <summary>
        /// 二宝生日
        /// </summary>
        public string KID_BIRTHDAY2 { get; set; }

        /// <summary>
        /// 从事行业
        /// </summary>
        public string ZC019 { get; set; }

        /// <summary>
        /// 收入范围
        /// </summary>
        public string ZC004 { get; set; }

        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string ZC016 { get; set; }

        /// <summary>
        /// 品牌编号
        /// </summary>
        public string BRAND_PREF { get; set; }

        /// <summary>
        /// 品类编号
        /// </summary>
        public string CLASS_PREF { get; set; }

        /// <summary>
        /// 省编号
        /// </summary>
        public string REGION { get; set; }

        /// <summary>
        /// 市编号
        /// </summary>
        public string TRANSPZONE { get; set; }

        /// <summary>
        /// 皮肤特征
        /// </summary>
        public string ZA003 { get; set; }

        /// <summary>
        /// 皮肤问题
        /// </summary>
        public string ZA004 { get; set; }

        /// <summary>
        /// 何处了解华美家
        /// </summary>
        public string INFO_WANTED { get; set; }

        /// <summary>
        /// BP号
        /// </summary>
        public string PARTNER { get; set; }

        public string OPENID { get; set; }
    }
}
