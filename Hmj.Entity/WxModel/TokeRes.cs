using Hmj.Entity.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Entity
{
    /// <summary>
    /// toke响应消息
    /// </summary>
    public class TokeRes: WxResBase
    {
        /// <summary>
        /// access_token
        /// </summary>
        [JsonProperty("access_token")]
        public string Access_Token { get; set; }

        /// <summary>
        /// 有效时间
        /// </summary>
        [JsonProperty("expires_in")]
        public int Expires_In { get; set; }
    }
}
