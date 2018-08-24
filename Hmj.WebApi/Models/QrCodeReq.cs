using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class QrCodeReq
    {
        /// <summary>
        /// 二维码类型，
        /// QR_SCENE为临时的整型参数值，
        /// QR_STR_SCENE为临时的字符串参数值，
        /// QR_LIMIT_SCENE为永久的整型参数值，
        /// QR_LIMIT_STR_SCENE为永久的字符串参数值
        /// </summary>
        [JsonProperty("action_name")]
        public string Action_Name { get; set; }

        /// <summary>
        /// 二维码详细信息
        /// object:{"scene_str": "test"}
        /// </summary>
        [JsonProperty("action_info")]
        public Dictionary<string, object> Action_Info { get; set; }
    }
}