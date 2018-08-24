namespace Hmj.Entity
{
    public class TenIP
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 详细信息
        /// </summary>
        public TenIPXy point { get; set; }

        /// <summary>
        /// 经纬度
        /// </summary>
        public TenIPDetail address_detail { get; set; }
    }
}
