using Newtonsoft.Json;
using System.Collections.Generic;

namespace Hmj.Entity.WxModel
{
    public class TemplateSend
    {
        /// <summary>
        /// openid
        /// </summary>
        [JsonProperty("touser")]
        public string Touser { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [JsonProperty("template_id")]
        public string Template_Id { get; set; }

        /// <summary>
        /// 点击推送模板所跳转的链接
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 详细参数
        /// </summary>
        [JsonProperty("data")]
        public Dictionary<string, TemplateData> Data { get; set; }

    }
}
