using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hmj.Entity.Entities
{
    public class FileRes : WxResBase
    {
        /// <summary>
        /// 该类型的素材的总数
        /// </summary>
        [JsonProperty("total_count")]
        public int Total_Count { get; set; }

        /// <summary>
        /// 本次调用获取的素材的数量
        /// </summary>
        [JsonProperty("item_count")]
        public int Item_Count { get; set; }

        /// <summary>
        /// 素材最后更新时间（仅仅素材才有该字段）
        /// </summary>
        [JsonProperty("update_time")]
        public string Update_Time { get; set; }

        /// <summary>
        /// 详情列表
        /// </summary>
        [JsonProperty("item")]
        public List<FileItemRes> Item { get; set; }
    }
}
