using Newtonsoft.Json;

namespace Hmj.Entity.WxModel
{
    public class TemplateData
    {
        /// <summary>
        /// 值
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// 顏色
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

    }
}
