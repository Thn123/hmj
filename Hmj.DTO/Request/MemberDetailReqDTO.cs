using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.DTO
{
    public class MemberDetailReqDTO
    {
        /// <summary>
        /// 得到openid
        /// </summary>
        public string OPENID { get; set; }

        /// <summary>
        /// 是否需要执行更新操作 0：不执行 1：执行
        /// 注意：如果传入0不执行更新操作 将无法得到AVA_POINTS和MEM_LEVEL字段
        /// </summary>
        public string IS_UPDATE { get; set; }
    }
}
