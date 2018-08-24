using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class PointHistory
    {
        /// <summary>
        /// 会员历史
        /// </summary>
       public List<PointHisList> Hise { get; set; }

        /// <summary>
        /// 当前积分
        /// </summary>
        public string Point { get; set; }
    }
}
