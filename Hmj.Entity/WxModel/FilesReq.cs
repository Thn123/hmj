using Newtonsoft.Json;

namespace Hmj.Entity.Entities
{
    /// <summary>
    /// 获取toke请求实体类
    /// </summary>
    public class FilesReq
    {
        /// <summary>
        /// 素材的类型，图片（image）、视频（video）、语音 （voice）、图文（news）
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// 从全部素材的该偏移位置开始返回，0表示从第一个素材 返回
        /// </summary>
        [JsonProperty("offset")]
        public int Offset { get; set; }

        /// <summary>
        /// 返回素材的数量，取值在1到20之间
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
