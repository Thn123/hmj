using Newtonsoft.Json;

namespace Hmj.Entity.Entities
{
    public class FileItemNewsRes 
    {
        /// <summary>
        ///  图文消息的标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        ///  图文消息的封面图片素材id（必须是永久mediaID）
        /// </summary>
        [JsonProperty("thumb_media_id")]
        public string Thumb_Media_Id { get; set; }

        /// <summary>
        /// 是否显示封面图
        /// </summary>
        [JsonProperty("show_cover_pic")]
        public string Show_Cover_Pic { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// 图文消息的摘要，仅有单图文消息才有摘要，多图文此处为空
        /// </summary>
        [JsonProperty("digest")]
        public string Digest { get; set; }

        /// <summary>
        /// 图文页的URL，或者，当获取的列表是图片素材列表时，该字段是图片的URL
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        
        /// <summary>
        /// 图文消息的原文地址，即点击“阅读原文”后的URL
        /// </summary>
        [JsonProperty("content_source_url")]
        public string Content_Source_Url { get; set; }
    }
}
