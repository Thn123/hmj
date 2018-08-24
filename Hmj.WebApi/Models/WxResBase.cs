using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hmj.WebApi.Models
{
    public class WxResBase
    {
        /// <summary>
        /// 报错信息code
        /// </summary>
        [JsonProperty("errcode")]
        public string Errcode { get; set; }

        /// <summary>
        /// 报错信息
        /// </summary>
        [JsonProperty("errmsg")]
        public string Errmsg { get; set; }
    }
}