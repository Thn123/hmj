using Newtonsoft.Json;

namespace Hmj.Entity.Entities
{
    public class FileItemRes 
    {
        /// <summary>
        /// 素材最后更新时间（仅仅素材才有该字段）
        /// </summary>
        [JsonProperty("media_id")]
        public string Media_Id { get; set; }

        /// <summary>
        /// 文件民称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 素材最后更新时间（非素材）
        /// </summary>
        [JsonProperty("update_time")]
        public string Update_Time { get; set; }

        /// <summary>
        /// 图文页的URL，或者，当获取的列表是图片素材列表时，该字段是图片的URL
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 仅仅图文有
        /// </summary>
        [JsonProperty("content")]
        public FileItemContentRes Content { get; set; }
    }
}
