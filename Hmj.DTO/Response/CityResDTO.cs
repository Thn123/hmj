using System.Collections.Generic;

namespace Hmj.DTO
{
    public class CityResDTO
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string NAME { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int CODE { get; set; }

        public List<CityResDTO> CHILD { get; set; }
}
}
