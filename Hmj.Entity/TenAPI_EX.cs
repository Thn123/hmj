namespace Hmj.Entity
{
    public class TenAPI_EX
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string formatted_address { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public TenAddress addressComponent { get; set; }
        
        /// <summary>
        /// 经纬度
        /// </summary>
        public TenLocal location { get;set;}
    }
}
